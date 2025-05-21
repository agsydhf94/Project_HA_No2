#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace HA
{
    [CustomPropertyDrawer(typeof(SoundIDSelectorAttribute))]
    public class SoundIDSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float buttonWidth = 25f;
            Rect fieldRect = new(position.x, position.y, position.width - buttonWidth - 4, position.height);
            Rect buttonRect = new(position.xMax - buttonWidth, position.y, buttonWidth, position.height);

            EditorGUI.BeginProperty(position, label, property);
            property.stringValue = EditorGUI.TextField(fieldRect, label, property.stringValue);

            if (GUI.Button(buttonRect, "Search"))
            {
                SoundIDSelectorPopup.Show(buttonRect, selected =>
                {
                    property.stringValue = selected;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif