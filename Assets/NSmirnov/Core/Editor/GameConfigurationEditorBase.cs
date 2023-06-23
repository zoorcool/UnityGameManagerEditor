using NSmirnov.Core.Foundation;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NSmirnov.Core.Editor
{
    public abstract class GameConfigurationEditorBase<C, P> : BaseEditor<C, P> where C : IGameConfiguration<P>, new() where P : IGameProperties, new()
    {
        protected ReorderableList langList;
        protected AttributeEditor properties;
        protected int langIndex;

        protected override void OnInit()
        {
            title = "Game Configuration";

            if (gameConfig.Properties.Langs == null)
                gameConfig.Properties.Langs = new List<string>();

            langIndex = -1;

            langList = new ReorderableList(gameConfig.Properties.Langs, typeof(string), true, true, true, true);
            langList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Langs");
            };
            langList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), gameConfig.Properties.Langs[index]);
            };
            langList.onAddCallback = (ReorderableList list) =>
            {
                gameConfig.Properties.Langs.Add("lang");
            };
            langList.onSelectCallback = (ReorderableList l) =>
            {
                if (properties != null)
                    properties.ClearSelection();
                langIndex = l.index;
            };

            properties = new AttributeEditor();
            properties.Init(gameConfig, "Property Attributes", gameConfig.Properties.Attributes, () =>
            {
                langIndex = -1;
            });
        }
        protected override void OnDraw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (langList != null)
                        langList.DoLayoutList();
                    GUILayout.Space(10);
                    if (properties != null)
                        properties.Draw();
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
                if (langIndex > -1 && langIndex < gameConfig.Properties.Langs.Count)
                    DrawLang();
                if (properties != null && properties.attributeCurrent != null)
                    properties.DrawAttribute();
            }
            GUILayout.EndHorizontal();
        }
        private void DrawLang()
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Name");
                gameConfig.Properties.Langs[langIndex] = EditorGUILayout.TextField(gameConfig.Properties.Langs[langIndex], GUILayout.MaxWidth(EditorSettings.RegularTextFieldWidth));
            }
            GUILayout.EndHorizontal();
        }
        public override void ClearSelection()
        {
            langIndex = -1;
            if (langList != null)
                langList.ClearSelection();
            if (properties != null)
                properties.ClearSelection();
        }
    }
}