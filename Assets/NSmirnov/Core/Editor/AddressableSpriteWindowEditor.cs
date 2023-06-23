using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEditor.AddressableAssets;
using System.Linq;

namespace NSmirnov.Core.Editor
{
    partial class AddresableSpriteWindowEditor : EditorWindow
    {
        public Action<AddressableAssetEntry> OnChange;
        private List<AddressableAssetGroup> groups = new List<AddressableAssetGroup>();

        private int groupIndex;
        private int folderIndex;
        private AddressableAssetGroup group;
        private AddressableAssetEntry folder;
        private Vector2 scrollPosition;

        public static AddresableSpriteWindowEditor Init()
        {
            var window = GetWindow(typeof(AddresableSpriteWindowEditor));
            window.titleContent = new GUIContent("Addresable Sprite Selected");
            window.minSize = window.maxSize = new Vector2(EditorSettings.SpriteWidth * 7, EditorSettings.SpriteWidth * 7);
            window.maximized = false;

            return (AddresableSpriteWindowEditor)window;
        }
        private void OnEnable()
        {
            var setting = AddressableAssetSettingsDefaultObject.Settings;
            groups = setting.groups.Where(_ => !_.Default).ToList();
        }
        private void OnGUI()
        {
            if (groups.Count > 0)
            {
                var groupIndexLast = groupIndex;

                groupIndex = EditorGUILayout.Popup(groupIndex, groups.Select(_ => _.Name).ToArray(), GUILayout.MaxWidth(EditorSettings.RegularComboBoxWidth));

                if (group == null || groupIndex != groupIndexLast)
                {
                    group = groups[groupIndex];
                }

                if (group.entries.FirstOrDefault().IsFolder)
                {
                    var folderIndexLast = folderIndex;
                    folderIndex = EditorGUILayout.Popup(folderIndex, group.entries.Select(_ => _.MainAsset.name).ToArray(), GUILayout.MaxWidth(EditorSettings.RegularComboBoxWidth));

                    if (folder == null || folderIndex != folderIndexLast)
                    {
                        folder = group.entries.ElementAt(folderIndex);
                    }
                }
                else
                {
                    folder = null;
                }

                DrawAvatar();
            }
        }
        private void DrawAvatar()
        {
            GUILayout.BeginHorizontal();
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                {
                    GUILayout.BeginVertical();
                    {
                        if (group != null)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                for (int i = 0; i < group.entries.Count; i++)
                                {
                                    if (i % 6 == 0)
                                    {
                                        GUILayout.EndHorizontal();
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Space(10);
                                    }

                                    var entry = group.entries.ElementAt(i);
                                    //Debug.Log(entry.MainAssetType);
                                    if (entry.MainAssetType == typeof(Texture2D))
                                    {
                                        DrawAddressableAssetEntryPreview(entry);
                                    }
                                }

                                if (folder != null)
                                {
                                    for(int i = 0; i < folder.SubAssets.Count; i++)
                                    {
                                        if (i % 6 == 0)
                                        {
                                            GUILayout.EndHorizontal();
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Space(10);
                                        }

                                        var entry = folder.SubAssets.ElementAt(i);
                                        //if (entry.labels.Contains("Sprite"))
                                        if (entry.MainAssetType == typeof(Texture2D))
                                        {
                                            DrawAddressableAssetEntryPreview(entry);
                                        }
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();
        }
        private void DrawAddressableAssetEntryPreview(AddressableAssetEntry entry)
        {
            Texture2D icon = EditorUtils.GetAddressableAssetEntryPreview(entry);

            if (icon)
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Box(icon, GUILayout.Height(EditorSettings.SpriteWidth), GUILayout.Width(EditorSettings.SpriteWidth));
                GUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Change", GUILayout.MaxWidth(EditorSettings.SpriteWidth)))
                {
                    OnChange?.Invoke(entry);
                    Close();
                }

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
        }
    }
}