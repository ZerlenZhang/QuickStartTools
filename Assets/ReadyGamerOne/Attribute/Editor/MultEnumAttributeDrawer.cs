using UnityEditor;
using UnityEngine;

namespace ReadyGamerOne.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(MultEnumAttribute))]
    public class MultEnumAttributeDrawer:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue
                , property.enumNames);
        }
    }
}