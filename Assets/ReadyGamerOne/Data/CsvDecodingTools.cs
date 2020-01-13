using System.IO;
using ReadyGamerOne.Global;
using UnityEditor;
using UnityEngine;

namespace ReadyGamerOne.Data
{
#pragma warning disable CS0414
#if UNITY_EDITOR
    
    public class CsvDecodingTools:IEditorTools
    {
        private static string csvDirPath = "";


        private static GUIStyle titleStyle = new GUIStyle
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };

        
        private static string Title = "CSV解析";
        private static void OnToolsGUI(string rootNs,string viewNs,string constNs,string dataNs,string autoDir,string scriptDir)
        {
            EditorGUILayout.Space();

            var generateDir = Application.dataPath + "/" + rootNs + "/" + dataNs + "/" + autoDir;
            
            EditorGUILayout.LabelField("csv数据类生成目录", rootNs + "/" + dataNs + "/" + autoDir);
            
            EditorGUILayout.Space();
            GUILayout.Label("请选择Csv文件所在目录",titleStyle);

            if(Directory.Exists(csvDirPath) )
                GUILayout.Label(csvDirPath);
            else
            {
                EditorGUILayout.HelpBox("请选择CSV目录路径", MessageType.Warning);
            }
            if (GUILayout.Button("设置CSV数据文件所在目录"))
                csvDirPath = EditorUtility.OpenFolderPanel("选择Csv文件所在目录", Directory.GetParent(Application.dataPath).FullName,"");
        
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("生成C#协议文件",GUILayout.Height(3*EditorGUIUtility.singleLineHeight)))
            {
                var consDir = Application.dataPath + "/" + rootNs + "/" + constNs + "/" + autoDir;
                if (!Directory.Exists(consDir))
                    Directory.CreateDirectory(consDir);
                if (!Directory.Exists(generateDir))
                    Directory.CreateDirectory(generateDir);
                
                if (Directory.Exists(csvDirPath))
                {
                    foreach (var fileFullPath in Directory.GetFiles(csvDirPath))
                    {
                        if(fileFullPath.EndsWith(".meta"))
                            continue;
                        if (fileFullPath.EndsWith(".csv") || fileFullPath.EndsWith(".CSV"))
                        {
                            CreatConfigFile(fileFullPath, generateDir,rootNs+"."+dataNs);                       
                        }
                    }
                    
         
                    Utility.FileUtil.ReCreateFileNameConstClassFromDir("FileName", consDir,Application.dataPath + "/Resources/ClassFile",rootNs+"."+constNs);               
                    AssetDatabase.Refresh();        //这里是一个点
                    Debug.Log("生成完成");
                }
                else
                {
                    Debug.LogError("生成失败——请正确设置所有路径");
                }
            }
        }

        private static void CreatConfigFile(string filePath, string writePath,string nameSpace)
        {
            var targetPath = Application.dataPath + "/Resources/ClassFile";

            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            targetPath = targetPath + "/" + fileName + ".csv";
            if (new FileInfo(targetPath).FullName != new FileInfo(filePath).FullName)
            {
                File.Copy(filePath, targetPath, true);
            }

            string className = fileName;
            StreamWriter sw = new StreamWriter(writePath + "/" + className + ".cs");

            sw.WriteLine("using UnityEngine;\nusing System.Collections;\nusing ReadyGamerOne.Data;\n");
            var ns = string.IsNullOrEmpty(nameSpace) ? "DefaultNamespace" : nameSpace;
            sw.WriteLine("namespace "+ns+"\n" +
                         "{\n\tpublic class " + className + " : CsvMgr");
            sw.WriteLine("\t{");

            var csr = new CsvStreamReader(filePath);
            
            sw.WriteLine("\t\tpublic const int "+className+"Count = "+csr.ColCount+";\n");
            for (int colNum = 1; colNum < csr.ColCount + 1; colNum++)
            {
                string fieldName = csr[1, colNum];
                string fieldType = csr[2, colNum];
                
                
                if(colNum==1)
                    sw.WriteLine("\t\tpublic override string ID => "+fieldName+".ToString();\n");
                
                sw.WriteLine("\t\t" + "public " + fieldType + " " + fieldName + ";" + "");
            }

            var toStringFunc = "\t\tpublic override string ToString()\n" +
                               "\t\t{\n" +
                               "\t\t\tvar ans=\"==《\t"+className+"\t》==\\n\" +\n";
            
            for (int colNum = 1; colNum < csr.ColCount + 1; colNum++)
            {
                string fieldName = csr[1, colNum];
                string fieldType = csr[2, colNum];
                
                
                toStringFunc += "\t\t\t\t\t" + "\"" + fieldName + "\" + \"\t\" + " + fieldName + "+\"\\n\" +\n";
            }

            toStringFunc = toStringFunc.Substring(0, toStringFunc.Length - "+\"\\n\" +\n".Length);
            
            toStringFunc += ";\n" +
                            "\t\t\treturn ans;\n" +
                            "\n\t\t}\n";
            
            sw.WriteLine(toStringFunc);
            
            sw.WriteLine("\t}\n" +
                         "}\n");

            sw.Flush();
            sw.Close();
        }
    }
#endif
    
}

