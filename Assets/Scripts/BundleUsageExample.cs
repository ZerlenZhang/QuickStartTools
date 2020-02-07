using System.Collections.Generic;
using Preview.Const;
using ReadyGamerOne.MemorySystem;
using ReadyGamerOne.Script;
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
            var obj = ResourceMgr.GetAsset<GameObject>(OtherResName.TestImage,OriginBundleKey.Prefab);
            AudioMgr.Instance.PlayBgm(OtherResName.Scene_1);
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
                ResourceMgr.GetAssetAsync<GameObject>(
                    ObjectsUsage[0].ObjectName,
                    ObjectsUsage[0].BundleNames,
                    obj => Instantiate(obj)));
        }
    }
}