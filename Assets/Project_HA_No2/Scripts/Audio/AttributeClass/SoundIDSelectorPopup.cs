#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HA
{
    public class SoundIDSelectorPopup : EditorWindow
    {
        private string search = "";
        private Action<string> onSelect;
        private List<string> soundIDs = new();

        public static void Show(Rect buttonRect, Action<string> onSelectCallback)
        {
            var window = CreateInstance<SoundIDSelectorPopup>();
            window.titleContent = new GUIContent("Select Sound ID");
            window.onSelect = onSelectCallback;
            window.soundIDs = AssetDatabase.FindAssets("t:SoundDataSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<SoundDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(asset => asset != null && !string.IsNullOrEmpty(asset.soundID))
                .Select(asset => asset.soundID)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            window.ShowAsDropDown(buttonRect, new Vector2(300, 400));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Search Sound ID", EditorStyles.boldLabel);
            search = EditorGUILayout.TextField(search);

            var filtered = string.IsNullOrWhiteSpace(search)
                ? soundIDs
                : soundIDs.Where(id => id.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            GUILayout.Space(5);
            var scroll = GUILayout.BeginScrollView(Vector2.zero);
            foreach (var id in filtered)
            {
                if (GUILayout.Button(id, EditorStyles.miniButton))
                {
                    onSelect?.Invoke(id);
                    Close();
                }
            }
            GUILayout.EndScrollView();
        }
    }
}
#endif