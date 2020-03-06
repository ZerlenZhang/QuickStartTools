using System;
using UnityEngine;

namespace ReadyGamerOne.EditorExtension
{
//#pragma warning disable CS0649
    [Serializable]
    public class TransformPathChooser
    {
        public const float LabelWidth = 0.4f;
        public const float ObjectFidldWidth = 0.2f;
        
        [SerializeField] private GameObject go;
        [SerializeField] private int selectedIndex;
        [SerializeField] private string path;

        public string Path => path;

        public string Name
        {
            get
            {
                var currentName = Path;
                if (currentName.Contains("/"))
                {
                    var select = currentName.LastIndexOf('/') + 1;
                    currentName = currentName.Substring(select, currentName.Length-select);
                    return currentName;
                }
                else
                    return Path;
            }
        }
    }
}