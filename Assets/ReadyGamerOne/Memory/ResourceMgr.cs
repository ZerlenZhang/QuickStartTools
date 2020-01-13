using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReadyGamerOne.Script;
using ReadyGamerOne.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using FileUtil = ReadyGamerOne.Utility.FileUtil;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.IO;
using ReadyGamerOne.Global;
using UnityEditor;

#endif


namespace ReadyGamerOne.MemorySystem
{
    /// <summary>
    /// 这个类提供关于内存的优化和管理
    /// 1、所有资源只会从Resources目录加载一次，再取的时候会从这个类的字典中取，尤其是一些预制体，经常频繁加载，使用这个类封装的Instantiate方法可以很好地解决这个问题
    /// 2、提供一些释放资源的接口
    /// 3、以后会提供关于AssetBubble的方法和接口
    /// 4、提供从Resources目录运行时加载一整个目录资源的接口，比如，加载某个文件夹下所有图片，音频
    /// </summary>
    public class ResourceMgr
#if UNITY_EDITOR
        : IEditorTools
#endif
    {
        #region Fields

        private static AssetBundleLoader assetBundleLoader;
        private static IHotUpdatePath pather;
        private static IOriginPathData originBundleConst;

        public static void Init(IHotUpdatePath pather, IOriginPathData originConstData)
        {
            if (null == pather)
                return;
            if (null == originConstData)
                return;
            ResourceMgr.originBundleConst = originConstData;
            ResourceMgr.pather = pather;
            assetBundleLoader = new AssetBundleLoader();
            MainLoop.Instance.StartCoroutine(assetBundleLoader.StartBundleManager(pather, originConstData));
        }

        #endregion

        public static void ShowDebugInfo()
        {
            Debug.Log("《AssetBundle加载情况》\n" + assetBundleLoader.DebugInfo());
        }

        #region Resources

        #region Private

        private static Dictionary<string, Object> sourceObjectDic;

        private static Dictionary<string, Object> SourceObjects
        {
            get
            {
                if (sourceObjectDic == null)
                    sourceObjectDic = new Dictionary<string, Object>();

                return sourceObjectDic;
            }
            set { sourceObjectDic = value; }
        }

        /// <summary>
        /// 从缓存中获取资源，否则采用默认方式初始化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultGetMethod"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static T GetSourceFromCache<T>(string key, Func<string, T> defaultGetMethod = null)
            where T : Object
        {
            if (SourceObjects.ContainsKey(key))
            {
                if (SourceObjects[key] == null)
                    throw new Exception("资源已经释放，但字典中引用亦然保留");

                var ans = SourceObjects[key] as T;
                if (ans == null)
                    Debug.LogWarning("资源引用存在，但类型转化失败，小心bug");

                return ans;
            }

            if (null == defaultGetMethod)
            {
                Debug.LogWarning("defaultGetMethod is null");
                return null;
            }

            var source = defaultGetMethod(key);
            if (source == null)
            {
                Debug.LogError("资源加载错误，错误路径：" + key);
                return null;
            }

            SourceObjects.Add(key, source);
            return source;
        }

        #endregion


        /// <summary>
        /// 根据路径获取资源引用，如果原来加载过，不会重复加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetSourceFromResources<T>(string path) where T : Object
        {
            return GetSourceFromCache(path, Resources.Load<T>);
        }

        /// <summary>
        /// 释放通过Resources.Load方法加载来的对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sourceObj"></param>
        /// <typeparam name="T"></typeparam>
        public static void ReleaseSourceFromResources<T>(string path, ref T sourceObj) where T : Object
        {
            if (!SourceObjects.ContainsKey(path))
            {
                Debug.LogWarning("资源字典中并不包含这个路径，注意路径是否错误：" + path);
                return;
            }

            var ans = SourceObjects[path] as T;
            if (ans == null)
            {
                Debug.LogWarning("该资源路径下Object转化为null，注意路径是否错误：" + path);
            }

            SourceObjects.Remove(path);
            Resources.UnloadAsset(ans);
            sourceObj = null;
        }

        public static void ReleaseSourceFromResources<T>(ref T sourceObj) where T : Object
        {
            if (!SourceObjects.ContainsValue(sourceObj))
            {
                Debug.LogWarning("资源字典中没有这个值，你真的没搞错？");
                return;
            }

            var list = new List<string>();
            foreach (var data in SourceObjects)
                if (data.Value == sourceObj)
                {
                    Resources.UnloadAsset(data.Value);
                    list.Add(data.Key);
                }

            foreach (var s in list)
            {
                SourceObjects.Remove(s);
            }

            list = null;


            sourceObj = null;
        }

        /// <summary>
        /// 释放游离资源
        /// </summary>
        public static void ReleaseUnusedAssets()
        {
            SourceObjects.Clear();
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 根据路径实例化对象
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Instantiate<T>(string path) where T : Object
        {
            return Object.Instantiate(GetSourceFromResources<T>(path));
        }

        /// <summary>
        /// 实例化GameObject
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject InstantiateGameObject(string path, Transform parent = null)
        {
            var source = GetSourceFromResources<GameObject>(path);
            Assert.IsTrue(source);
            return Object.Instantiate(source, parent);
        }

        public static GameObject InstantiateGameObject(string path, Vector3 pos, Quaternion quaternion,
            Transform parent = null)
        {
            var source = GetSourceFromResources<GameObject>(path);
            Assert.IsTrue(source);
            return Object.Instantiate(source, pos, quaternion, parent);
        }

        public static GameObject InstantiateGameObject(string path, Vector3 pos, Transform parent = null)
        {
            var source = GetSourceFromResources<GameObject>(path);
            Assert.IsTrue(source);
            return Object.Instantiate(source, pos, Quaternion.identity, parent);
        }


        /// <summary>
        /// 从Resources目录中动态加载指定目录下所有内容
        /// </summary>
        /// <param name="nameClassType">目录下所有资源的名字要作为一个静态类的public static 成员，这里传递 这个静态类的Type</param>
        /// <param name="dirPath">从Resources开始的根目录，比如“Audio/"</param>
        /// <param name="onLoadAsset">加载资源时调用的委托，不能为空</param>
        /// <typeparam name="TAssetType">加载资源的类型</typeparam>
        /// <exception cref="Exception">委托为空会抛异常</exception>
        public static void LoadAssetFromResourceDir<TAssetType>(Type nameClassType, string dirPath = "",
            Action<string, TAssetType> onLoadAsset = null)
            where TAssetType : Object
        {
            var infoList = nameClassType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fieldInfo in infoList)
            {
                string assetName = fieldInfo.GetValue(null) as string;

                var res = GetSourceFromResources<TAssetType>(dirPath + assetName);

                if (onLoadAsset == null)
                    throw new Exception("onLoadAsset为 null, 那你加载资源干啥？？ ");

                onLoadAsset.Invoke(assetName, res);
            }
        }

        #endregion

        #region AssetBundle

        /// <summary>
        /// 异步从本地读取AssetBundle
        /// 条件：1、AB包没有做过任何加密
        ///       2、AB包存储在本地
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="crc"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IEnumerator LoadAssetBundleFromLocalAsync(string filePath, uint crc = 0u, ulong offset = 0ul)
        {
            yield return AssetBundle.LoadFromFileAsync(filePath, crc, offset);
        }

        /// <summary>
        /// 同步从本地读取AssetBundle
        /// 条件：1、AB包没有做过任何加密
        ///       2、AB包存储在本地
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="crc"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static AssetBundle LoadAssetBundleFromLocal(string filePath, uint crc = 0u, ulong offset = 0ul)
        {
            return AssetBundle.LoadFromFile(filePath, crc, offset);
        }


        /// <summary>
        /// 异步从streaming加载物体并使用，优先使用缓存
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="objName"></param>
        /// <param name="onGetObj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerator GetAssetFromBundleAsync<T>(string bundleName, string objName, Action<T> onGetObj)
            where T : Object
        {
            if (null == pather)
                throw new Exception("没有初始化MemoryMgr.Pather");

            Assert.IsNotNull(onGetObj);

            //如果缓存中有，就直接使用缓存
            var temp = GetSourceFromCache<T>($"{bundleName}_{objName}");
            if (null != temp)
            {
                onGetObj(temp);
                yield break;
            }

            //缓存没有，使用加载器加载
            yield return assetBundleLoader.GetBundleAsync(bundleName,
                ab => GetAssetFormBundleAsync(ab, objName, onGetObj));

            IEnumerator GetAssetFormBundleAsync(AssetBundle assetBundle, string _objName, Action<T> _onGetObj)
            {
                var objRequest = assetBundle.LoadAssetAsync<T>(_objName);
                yield return objRequest;

                //添加到缓存
                var ans = objRequest.asset as T;
                if (null == ans)
                    throw new Exception("Get Asset is null");
                SourceObjects.Add($"{bundleName}_{objName}", ans);

                //使用物品
                _onGetObj((T) objRequest.asset);
            }
        }

        /// <summary>
        /// 同步加载Self资源，优先使用缓存
        /// </summary>
        /// <param name="objKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetAssetFromStreaming<T>(string objKey)
            where T : Object
        {
            return GetAssetFromBundle<T>(OriginBundleKey.Self, objKey);
        }

        /// <summary>
        /// 同步加载资源，优先使用缓存
        /// </summary>
        /// <param name="bundleNameKey"></param>
        /// <param name="objKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetAssetFromBundle<T>(string bundleNameKey, string objKey)
            where T : Object
        {
            if (null == pather)
                throw new Exception("没有初始化MemoryMgr.Pather");

            if (originBundleConst == null)
                throw new Exception("originBundleConst is null");


            return GetSourceFromCache(objKey,
                key =>
                {
                    var ab = assetBundleLoader.GetBundle(bundleNameKey);
                    Assert.IsTrue(ab);
                    return ab.LoadAsset<T>(key);
                });
        }

        #endregion

        #region IEditorTools

#if UNITY_EDITOR
        static string Title = "AssetBundle打包";
        private static string assetToBundleDir = "未设置";
        private static string outputDir = "未设置";
        private static BuildAssetBundleOptions assetBundleOptions = 0;
        private static BuildTarget buildTarget = BuildTarget.StandaloneWindows;
        private static bool clearOutputDir = false;

        private static bool createPathDataClass = false;

        private static bool useForRuntime = true;

        private static string streamingAbName = "self";
        private static bool useWeb = false;
        private static List<string> assetBundleNames = new List<string>();
//        private static List<string> _assetNames = new List<string>();

        static void OnToolsGUI(string rootNs, string viewNs, string constNs, string dataNs, string autoDir,
            string scriptDir)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("打包目录", assetToBundleDir);
            if (GUILayout.Button("设置要打包的目录"))
            {
                assetToBundleDir = EditorUtility.OpenFolderPanel("选择需要打包的目录", Application.dataPath, "");
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("输出目录", outputDir);
            if (GUILayout.Button("设置输出目录"))
            {
                outputDir = EditorUtility.OpenFolderPanel("选择输出目录", Application.dataPath, "");
            }

            EditorGUILayout.Space();
            assetBundleOptions = (BuildAssetBundleOptions) EditorGUILayout.EnumFlagsField("打包选项", assetBundleOptions);

            buildTarget = (BuildTarget) EditorGUILayout.EnumPopup("目标平台", buildTarget);
            clearOutputDir = EditorGUILayout.Toggle("清空生成目录", clearOutputDir);
            EditorGUILayout.Space();

            useForRuntime = EditorGUILayout.Foldout(useForRuntime, "使用用于生成运行时直接使用的AB包");
            if (useForRuntime)
            {
                EditorGUILayout.LabelField("生成HotUpdatePath.cs路径",
                    Application.dataPath + "/" + rootNs + "/" + constNs + "/" + autoDir + "/HotUpdatePath.cs");
                useWeb = EditorGUILayout.Toggle("是否使用网络", useWeb);
                EditorGUILayout.LabelField("游戏自身AB包名字", streamingAbName);

                EditorGUILayout.Space();
                if (GUILayout.Button("重新生成常量类【会覆盖】"))
                {
                    if (!outputDir.Contains(Application.streamingAssetsPath))
                    {
                        Debug.LogError("运行时使用的AB包必须在StreamingAssets目录下");
                        return;
                    }

                    if (assetBundleNames.Count == 0)
                    {
                        Debug.LogError("AB包数组未空");
                        return;
                    }

                    CreatePathDataClass(rootNs, constNs, autoDir, assetBundleNames);
                    AssetDatabase.Refresh();
                    Debug.Log("生成完成");
                }
            }


            if (GUILayout.Button("开始打包", GUILayout.Height(3 * EditorGUIUtility.singleLineHeight)))
            {
                if (createPathDataClass)
                {
                    if (!outputDir.Contains(Application.streamingAssetsPath))
                    {
                        Debug.LogError("运行时使用的AB包必须在StreamingAssets目录下");
                        return;
                    }
                }
                if (assetBundleNames.Count != 0)
                    assetBundleNames.Clear();
                if (!Directory.Exists(assetToBundleDir))
                {
                    Debug.LogError("打包目录设置异常");
                    return;
                }

                if (!Directory.Exists(outputDir))
                {
                    Debug.LogError("输出目录设置异常");
                    return;
                }

                if (clearOutputDir)
                {
                    if (Directory.Exists(outputDir))
                    {
                        //获取指定路径下所有文件夹
                        string[] folderPaths = Directory.GetDirectories(outputDir);

                        foreach (string folderPath in folderPaths)
                            Directory.Delete(folderPath, true);
                        //获取指定路径下所有文件
                        string[] filePaths = Directory.GetFiles(outputDir);

                        foreach (string filePath in filePaths)
                            File.Delete(filePath);
                    }
                }

                var builds = new List<AssetBundleBuild>();
                foreach (var dirPath in System.IO.Directory.GetDirectories(assetToBundleDir))
                {
                    var dirName = new DirectoryInfo(dirPath).Name;
                    var paths = new List<string>();
                    var assetNames = new List<string>();
                    FileUtil.SearchDirectory(dirPath,
                        fileInfo =>
                        {
                            if (fileInfo.Name.EndsWith(".meta"))
                                return;
                            assetNames.Add(Path.GetFileNameWithoutExtension(fileInfo.FullName));
                            var fileLoadPath = fileInfo.FullName.Replace("\\", "/")
                                .Replace(Application.dataPath, "Assets");
                            var ai = AssetImporter.GetAtPath(fileLoadPath);
                            ai.assetBundleName = dirName;
                            paths.Add(fileLoadPath);
                        }, true);

                    assetBundleNames.Add(dirName);
                    FileUtil.CreateConstClassByDictionary(
                        dirName+"Name",
                        Application.dataPath+"/"+rootNs+"/"+constNs,
                        rootNs+"."+constNs,
                        assetNames.ToDictionary(name=>name));
                    builds.Add(new AssetBundleBuild
                    {
                        assetNames = paths.ToArray(),
                        assetBundleName = dirName
                    });
                }
                if (createPathDataClass)
                {
                    CreatePathDataClass(rootNs, constNs, autoDir, assetBundleNames);
                }
                BuildPipeline.BuildAssetBundles(outputDir, builds.ToArray(), assetBundleOptions, buildTarget);
                AssetDatabase.Refresh();
                Debug.Log("打包完成");
            }
        }


        /// <summary>
        /// 创建用于使用AB包管理资源的路径定义类
        /// </summary>
        /// <param name="rootNs"></param>
        /// <param name="constNs"></param>
        /// <param name="autoDir"></param>
        private static void CreatePathDataClass(string rootNs, string constNs, string autoDir, List<string> names)
        {
            var outputDir = ResourceMgr.outputDir.Replace(Application.streamingAssetsPath, "");
            var mainAssetBundleName = new DirectoryInfo(ResourceMgr.outputDir).Name;
            var content = "";
            var otherClassBody = "\n";

            #region OriginBundleKey

            otherClassBody += "\tpublic class OriginBundleKey : ReadyGamerOne.MemorySystem.OriginBundleKey\n" +
                              "\t{\n";
            foreach (var name in names)
            {
                if (name.GetAfterLastChar('/') == "self")
                    continue;
                otherClassBody += "\t\tpublic const string " + name.Trim().GetAfterLastChar('/') + " = " +
                                  "@\"" + name.Trim() + "\";\n";
            }

            otherClassBody += "\t}\n";

            #endregion

            #region OriginBundleConst

            otherClassBody += "\n\tpublic class OriginBundleConst : OriginBundleConst<OriginBundleConst>\n" +
                              "\t{\n";

            #region KeyToName

            otherClassBody += "\t\tprivate Dictionary<string,string> _keyToName = new Dictionary<string,string>\n" +
                              "\t\t{\n" +
                              "\t\t\t{\"Self\" , @\"self\"},\n";
            foreach (var name in names)
            {
                if (name.GetAfterLastChar('/') == "self")
                    continue;
                otherClassBody += "\t\t\t{\"" + name.Trim().GetAfterLastChar('/') + "\" , " +
                                  "@\"" + name.Trim().ToLower() + "\"},\n";
            }

            otherClassBody += "\t\t};\n";
            otherClassBody += "\t\tpublic override Dictionary<string, string> KeyToName => _keyToName;\n";

            #endregion

            #region KeyToPath

            otherClassBody += "\t\tprivate Dictionary<string,string> _keyToPath = new Dictionary<string,string>\n" +
                              "\t\t{\n" +
                              "\t\t\t{\"Self\" , Path.Combine(Application.streamingAssetsPath + @\"" + outputDir +
                              "\", @\"self\")},\n";
            foreach (var name in names)
            {
                if (name.GetAfterLastChar('/') == "self")
                    continue;
                otherClassBody += "\t\t\t{\"" + name.Trim().GetAfterLastChar('/') + "\" , " +
                                  "Path.Combine(Application.streamingAssetsPath + @\"" + outputDir + "\", @\"" +
                                  name.Trim().ToLower() + "\")},\n";
            }

            otherClassBody += "\t\t};\n";
            otherClassBody += "\t\tpublic override Dictionary<string, string> KeyToPath => _keyToPath;\n";

            #endregion

            #region NameToPath

            otherClassBody += "\t\tprivate Dictionary<string,string> _nameToPath = new Dictionary<string,string>\n" +
                              "\t\t{\n" +
                              "\t\t\t{@\"self\" , Path.Combine(Application.streamingAssetsPath + @\"" + outputDir +
                              "\", @\"self\")},\n";
            foreach (var name in names)
            {
                if (name.GetAfterLastChar('/') == "self")
                    continue;
                otherClassBody += "\t\t\t{@\"" + name.Trim().ToLower() + "\" , " +
                                  "Path.Combine(Application.streamingAssetsPath + @\"" + outputDir + "\", @\"" +
                                  name.Trim().ToLower() + "\")},\n";
            }

            otherClassBody += "\t\t};\n";
            otherClassBody += "\t\tpublic override Dictionary<string, string> NameToPath => _nameToPath;\n";

            #endregion

            otherClassBody += "\t}\n";

            #endregion

            if (useWeb)
            {
                content =
                    "\t\tpublic string OriginMainManifest => @Path.Combine(Application.streamingAssetsPath + @\"" +
                    outputDir + "\", \"" +
                    mainAssetBundleName + "\");\n" +
                    "\t\tpublic string LocalMainPath => @Path.Combine(Application.persistentDataPath, \"AssetBundles\");\n" +
                    "\t\tpublic string WebServeMainPath => @\"file:/C:\\Users\\ReadyGamerOne\\Downloads\\webserver\";\n" +
                    "\t\tpublic string WebServeMainManifest => WebServeMainPath + \"\\\\ManifestFile\";\n" +
                    "\t\tpublic string WebServeVersionPath => WebServeMainPath + \"\\\\ServeVersion.html\";\n" +
                    "\t\tpublic string WebServeBundlePath => WebServeMainPath + \"\\\\AssetBundles\";\n" +
                    "\t\tpublic string WebServeConfigPath => WebServeMainPath + \"\\\\ServeConfig\";\n" +
                    "\t\tpublic Func<string, string> GetServeConfigPath => version =>$\"{WebServeConfigPath}/{version}.html\";\n" +
                    "\t\tpublic Func<string, string, string> GetServeBundlePath => (bundleName,bundleVersion)=>$\"{WebServeBundlePath}/{bundleVersion}/{bundleName}\";\n" +
                    "\t\tpublic Func<string, string, string> GetLocalBundlePath => (bundleName,bundleVersion)=>$\"{LocalMainPath}/{bundleVersion}/{bundleName}\";\n";
            }
            else
            {
                content =
                    "\t\tpublic string OriginMainManifest => @Path.Combine(Application.streamingAssetsPath + @\"" +
                    outputDir + "\", \"" +
                    mainAssetBundleName + "\");\n" +
                    "\t\tpublic string LocalMainPath => null;\n" +
                    "\t\tpublic string WebServeMainPath => null;\n" +
                    "\t\tpublic string WebServeMainManifest => null;\n" +
                    "\t\tpublic string WebServeVersionPath => null;\n" +
                    "\t\tpublic string WebServeBundlePath => null;\n" +
                    "\t\tpublic string WebServeConfigPath => null;\n" +
                    "\t\tpublic Func<string, string> GetServeConfigPath => null;\n" +
                    "\t\tpublic Func<string, string, string> GetServeBundlePath => null;\n" +
                    "\t\tpublic Func<string, string, string> GetLocalBundlePath => null;\n";
            }

            FileUtil.CreateClassFile(
                "HotUpdatePathData",
                rootNs + "." + constNs,
                Application.dataPath + "/" + rootNs + "/" + constNs,
                "Singleton<HotUpdatePathData>, IHotUpdatePath",
                "使用AB包管理资源时必须的定义路径的常量类",
                content,
                true,
                false,
                "using ReadyGamerOne.MemorySystem;\n" +
                "using UnityEngine;\n" +
                "using System.IO;\n" +
                "using System;\n" +
                "using System.Collections.Generic;\n" +
                "using ReadyGamerOne.Common;\n",
                otherClassBody: otherClassBody);
        }


#endif

        #endregion
    }
}