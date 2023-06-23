using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditorInternal;
using UnityEngine;

namespace NSmirnov.Core.Editor
{
    public class EditorUtils
    {
        public static ReorderableList SetupReorderableList<T>(
            string headerText,
            List<T> elements,
            Action<Rect, T> drawElement,
            Action<T> selectElement,
            Action createElement,
            Action<T> removeElement,
            bool draggable = false,
            bool displayHeader = true,
            bool displayAddButton = true,
            bool displayRemoveButton = true
            )
        {
            var list = new ReorderableList(elements, typeof(T), draggable, displayHeader, displayAddButton, displayRemoveButton);

            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, headerText);
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = elements[index];
                drawElement(rect, element);
            };

            list.onSelectCallback = (ReorderableList l) =>
            {
                var selectedElement = elements[list.index];
                selectElement(selectedElement);
            };

            if (createElement != null)
            {
                list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
                {
                    createElement();
                };
            }

            list.onRemoveCallback = (ReorderableList l) =>
            {
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No"))
                {
                    var element = elements[l.index];
                    removeElement(element);
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                }
            };

            return list;
        }
        public static Texture2D GetAddressableAssetEntryPreview(AddressableAssetEntry entry)
        {
            if (!AssetPreview.IsLoadingAssetPreview(entry.MainAsset.GetInstanceID()))
            {
                return AssetPreview.GetAssetPreview(entry.MainAsset);
            }

            return null;
        }
    }
}