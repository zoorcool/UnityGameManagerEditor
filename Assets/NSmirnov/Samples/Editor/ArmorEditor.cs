using NSmirnov.Core.Editor;
using NSmirnov.Core.Foundation;
using NSmirnov.Samples.Foundation;
using NSmirnov.Samples.Foundation.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditorInternal;
using UnityEngine;

namespace NSmirnov.Samples.Editor
{
    public class ArmorEditor : BaseEditor<GameConfiguration, GameProperties>
    {
        private ReorderableList classList, armorList;
        private Armor armorCurrent = null;
        protected override void OnInit()
        {
            title = "Armors";

            classList = EditorUtils.SetupReorderableList("Classes", Enum.GetValues(typeof(ArmorClass)).Cast<ArmorClass>().ToList(), (rect, x) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), x.GetDescription());
            },
            (x) =>
            {
                CreateArmorList(x);
            },
            () => { }, (x) => { }, displayAddButton: false, displayRemoveButton: false);
        }
        private void CreateArmorList(ArmorClass armorClass)
        {
            armorCurrent = null;

            if (gameConfig.Armors == null)
                gameConfig.Armors = new List<Armor>();

            armorList = EditorUtils.SetupReorderableList("Armor List", gameConfig.Armors.Where(_ => _.Subclass == armorClass).ToList(), (rect, x) =>
            {
                string title = "Armor";

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
                armorCurrent = x;
            },
            () =>
            {
                Armor armor = new Armor
                {
                    Id = Guid.NewGuid().ToString(),
                    Class = ItemClass.Armor,
                    Subclass = armorClass,
                    Name = new List<Lang>
                    {
                        new Lang(gameConfig.Properties.Langs.FirstOrDefault(), armorClass.GetDescription())
                    }
                };

                gameConfig.Armors.Add(armor);

                CreateArmorList(armorClass);
            },
            (x) =>
            {
                armorCurrent = null;
            });
        }
        protected override void OnDraw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (classList != null)
                        classList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (armorList != null)
                        armorList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
                if (armorCurrent != null)
                    DrawWeapon();
            }
            GUILayout.EndHorizontal();
        }
        private void DrawWeapon()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    EditorGUILayout.HelpBox($"Class: {armorCurrent.Class}{Environment.NewLine}Subclass: {armorCurrent.Subclass}", MessageType.None, true);

                    if (!string.IsNullOrEmpty(armorCurrent.EntryGuid))
                    {

                        var setting = AddressableAssetSettingsDefaultObject.Settings;
                        if (setting != null)
                        {
                            var entry = setting.FindAssetEntry(armorCurrent.EntryGuid);
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
                            armorCurrent.EntryGuid = entry.guid;
                        };
                    }

                    GUILayout.Space(10);
                    for (int i = 0; i < armorCurrent.Name.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel($"Имя [{armorCurrent.Name[i].Key}]");
                            armorCurrent.Name[i].Value = EditorGUILayout.TextField(armorCurrent.Name[i].Value);
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel($"{armorCurrent.GetMemberDisplayName("Defence")}[Min]");
                        armorCurrent.Defence.Min = EditorGUILayout.IntField(armorCurrent.Defence.Min);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel($"{armorCurrent.GetMemberDisplayName("Defence")}[Max]");
                        armorCurrent.Defence.Max = EditorGUILayout.IntField(armorCurrent.Defence.Max);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        public override void ClearSelection()
        {
            armorCurrent = null;
        }
    }
}