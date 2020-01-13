using System.Collections.Generic;
using System.Linq;
using Preview.Const;
using ReadyGamerOne.MemorySystem;
using UnityEngine;
using UnityEngine.UI;
using OriginBundleKey = Preview.Const.OriginBundleKey;

public class BundleUsageExample : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectUsage
    {
        public string BundleNames;
        public string ObjectName;
    }

    public List<ObjectUsage> ObjectsUsage;
    // Use this for initialization
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            var obj = ResourceMgr.GetAssetFromBundle<GameObject>(
                OriginBundleKey.Prefab, 
                PrefabName.TestImage);
            
            var itemObj = Instantiate(obj, transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    public void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ResourceMgr.ShowDebugInfo();
        }
        if (GUILayout.Button("LoadStuff"))
        {
            StartCoroutine(
                ResourceMgr.GetAssetFromBundleAsync<GameObject>(
                    ObjectsUsage[0].BundleNames,
                    ObjectsUsage[0].ObjectName,
                    obj => Instantiate(obj)));
        }
    }
}