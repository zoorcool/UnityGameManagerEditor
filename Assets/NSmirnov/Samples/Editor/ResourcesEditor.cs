using NSmirnov.Core.Editor;
using NSmirnov.Core.Foundation;
using NSmirnov.Samples.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditorInternal;
using UnityEngine;

namespace NSmirnov.Samples.Editor
{
    public class ResourcesEditor : BaseEditor<GameConfiguration, GameProperties>
    {
        private ReorderableList resourcesList;
        private Foundation.Items.Resources resourcesCurrent = null;
        protected override void OnInit()
        {
            title = "Resources";

            CreateResourcesList();
        }
        private void CreateResourcesList()
        {
            resourcesList = EditorUtils.SetupReorderableList("Resources List", gameConfig.Resources, (rect, x) =>
            {
                string title = "Resources";

                if (x.Name != null)
                {
                    title = x.Name.FirstOrDefault(_ => _.Key == gameConfig.Properties.Langs.First())?.Value;
                }

                if (!string.IsNullOrEmpty(x.EntryGuid))
                {
                    var setting = AddressableAssetSettingsDefaultObject.Settings;
                    if (setting != null)
                    {
                        var entry = setting.FindAssetEntry(x.EntryGuid);
                        if (entry != null)
                        {
                            DrawAddressableAssetEntryPreview(new Rect(rect.x, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), entry);
                        }
                    }
                }

                EditorGUI.LabelField(new Rect(rect.x + EditorGUIUtility.singleLineHeight + 5, rect.y, 200, EditorGUIUtility.singleLineHeight), title);
            },
            (x) =>
            {
                resourcesCurrent = x;
            },
            () =>
            {
                Foundation.Items.Resources resources = new Foundation.Items.Resources();
                resources.Id = Guid.NewGuid().ToString();
                resources.Name = new List<Lang>
                {
                                new Lang(gameConfig.Properties.Langs.FirstOrDefault())
                };

                if (gameConfig.Resources == null)
                    gameConfig.Resources = new List<Foundation.Items.Resources>();
                gameConfig.Resources.Add(resources);

                CreateResourcesList();
            },
            (x) =>
            {
                resourcesCurrent = null;
            });
        }
        protected override void OnDraw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (resourcesList != null)
                        resourcesList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
                if (resourcesCurrent != null)
                    DrawResources();
            }
            GUILayout.EndHorizontal();
        }
        private void DrawResources()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (!string.IsNullOrEmpty(resourcesCurrent.EntryGuid))
                    {

                        var setting = AddressableAssetSettingsDefaultObject.Settings;
                        if (setting != null)
                        {
                            var entry = setting.FindAssetEntry(resourcesCurrent.EntryGuid);
                            if (entry != null)
                            {
                                DrawAddressableAssetEntryPreview(entry, 96, 96);
                            }
                        }
                    }
                    if (GUILayout.Button("Selected sprite"))
                    {
                        var win = AddresableSpriteWindowEditor.Init();
                        win.OnChange = entry =>
                        {
                            resourcesCurrent.EntryGuid = entry.guid;
                        };
                    }

                    GUILayout.Space(10);
                    for (int i = 0; i < resourcesCurrent.Name.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel($"Имя [{resourcesCurrent.Name[i].Key}]");
                            resourcesCurrent.Name[i].Value = EditorGUILayout.TextField(resourcesCurrent.Name[i].Value);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        public override void ClearSelection()
        {
            resourcesCurrent = null;
        }
    }
}