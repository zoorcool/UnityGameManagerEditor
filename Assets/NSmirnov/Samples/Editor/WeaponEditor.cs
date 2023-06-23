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
    public class WeaponEditor : BaseEditor<GameConfiguration, GameProperties>
    {
        private ReorderableList weaponList;
        private Weapon weaponCurrent = null;
        protected override void OnInit()
        {
            title = "Weapons";

            CreateWeaponList();
        }

        private void CreateWeaponList()
        {
            weaponList = EditorUtils.SetupReorderableList("Weapons List", gameConfig.Weapons, (rect, x) =>
            {
                string title = "Weapon";

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
                weaponCurrent = x;
            },
            () =>
            {
                Weapon weapon = new Weapon();
                weapon.Id = Guid.NewGuid().ToString();
                weapon.Name = new List<Lang>
                {
                    new Lang(gameConfig.Properties.Langs.FirstOrDefault())
                };

                if (gameConfig.Weapons == null)
                    gameConfig.Weapons = new List<Weapon>();
                gameConfig.Weapons.Add(weapon);

                CreateWeaponList();
            },
            (x) =>
            {
                weaponCurrent = null;
            });
        }
        protected override void OnDraw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (weaponList != null)
                        weaponList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
                if (weaponCurrent != null)
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
                    if (!string.IsNullOrEmpty(weaponCurrent.EntryGuid))
                    {

                        var setting = AddressableAssetSettingsDefaultObject.Settings;
                        if (setting != null)
                        {
                            var entry = setting.FindAssetEntry(weaponCurrent.EntryGuid);
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
                            weaponCurrent.EntryGuid = entry.guid;
                        };
                    }

                    GUILayout.Space(10);
                    for (int i = 0; i < weaponCurrent.Name.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel($"Имя [{weaponCurrent.Name[i].Key}]");
                            weaponCurrent.Name[i].Value = EditorGUILayout.TextField(weaponCurrent.Name[i].Value);
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel($"{weaponCurrent.GetMemberDisplayName("Damage")}[Min]");
                        weaponCurrent.Damage.Min = EditorGUILayout.IntField(weaponCurrent.Damage.Min);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PrefixLabel($"{weaponCurrent.GetMemberDisplayName("Damage")}[Max]");
                        weaponCurrent.Damage.Max = EditorGUILayout.IntField(weaponCurrent.Damage.Max);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        public override void ClearSelection()
        {
            weaponCurrent = null;
        }
    }
}