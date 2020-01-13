using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ScrptableTest.ScriptableObjects
{
    public class TestAsset : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("ReadyGamerOne/Create/TestAsset")]
        public static void CreateAsset()
        {
            string[] strs = Selection.assetGUIDs;

            string path = AssetDatabase.GUIDToAssetPath(strs[0]);

            if (path.Contains("."))
            {
                path=path.Substring(0, path.LastIndexOf('/'));
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(CreateInstance<TestAsset>(), path + "/NewTestAsset.asset");
            AssetDatabase.Refresh();

            Selection.activeObject = AssetDatabase.LoadAssetAtPath<TestAsset>(path + "/NewTestAsset.asset");    
        }        
#endif

        
        
        
        public string testString;


        private void OnDestroy()
        {
            Debug.Log("OnDestory   "+name);
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable   "+name);
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable   "+name);
        }

        private void OnValidate()
        {
            Debug.Log("OnValidate   "+name);
        }

        private void Awake()
        {
            Debug.Log("Awake   "+name);
        }
    }
}