using NSmirnov.Core.Foundation;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace NSmirnov.Core.Editor
{
    public class AttributeEditor
    {
        private List<Foundation.Attribute> attributes;
        private ReorderableList attributeList, langAttributeList;
        public Foundation.Attribute attributeCurrent { get; private set; }
        private Lang langAttributeCurrent;
        public void Init<T>(IGameConfiguration<T> gameConfig, string headerText, List<Foundation.Attribute> attributes, UnityAction onSelectElement) where T : IGameProperties, new()
        {
            this.attributes = attributes;

            attributeList = EditorUtils.SetupReorderableList(headerText, this.attributes, (rect, x) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), x.Name);
            },
            (x) =>
            {
                onSelectElement?.Invoke();

                langAttributeCurrent = null;

                attributeCurrent = x;

                if (x is LangAttribute)
                {
                    var langAttinbute = x as LangAttribute;

                    langAttributeList = EditorUtils.SetupReorderableList("Lang", langAttinbute.Value, (rect, x) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), x.Key);
                    },
                    (x) =>
                    {
                        langAttributeCurrent = x;
                    },
                    () =>
                    {
                        var menu = new GenericMenu();
                        foreach (var item in gameConfig.Properties.Langs)
                        {
                            menu.AddItem(new GUIContent(item), false, lang =>
                            {
                                var item = new Lang(lang.ToString());
                                langAttinbute.Value.Add(item);
                            }, item);
                        }
                        menu.ShowAsContext();
                    },
                    (x) =>
                    {
                        langAttributeCurrent = null;
                    });
                }
            },
            () =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Bool"), false, CreateAttributeCallback, "Bool");
                menu.AddItem(new GUIContent("Integer"), false, CreateAttributeCallback, "Integer");
                menu.AddItem(new GUIContent("Double"), false, CreateAttributeCallback, "Double");
                menu.AddItem(new GUIContent("String"), false, CreateAttributeCallback, "String");
                menu.AddItem(new GUIContent("Lang"), false, CreateAttributeCallback, "Lang");
                menu.ShowAsContext();
            },
            (x) =>
            {
                attributeCurrent = null;
            }, true);
        }
        private void CreateAttributeCallback(object obj)
        {
            Attribute attribute = null;
            switch ((string)obj)
            {
                case "Bool":
                    attribute = new BoolAttribute();
                    break;

                case "Integer":
                    attribute = new IntAttribute();
                    break;

                case "Double":
                    attribute = new DoubleAttribute();
                    break;

                case "String":
                    attribute = new StringAttribute();
                    break;

                case "Lang":
                    attribute = new LangAttribute();
                    break;
            }
            attributes.Add(attribute);
        }
        public void Draw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(3);
                GUILayout.BeginVertical();
                {
                    if (attributeList != null)
                        attributeList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.Space(3);
            }
            GUILayout.EndHorizontal();
        }
        public void DrawAttribute(bool displayDefaultValue = true)
        {
            var oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = EditorSettings.RegularLabelWidth;
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                if (attributeCurrent is BoolAttribute)
                    GUILayout.Label("Bool", EditorStyles.boldLabel);
                else if (attributeCurrent is IntAttribute)
                    GUILayout.Label("Integer", EditorStyles.boldLabel);
                else if (attributeCurrent is DoubleAttribute)
                    GUILayout.Label("Double", EditorStyles.boldLabel);
                else if (attributeCurrent is StringAttribute)
                    GUILayout.Label("String", EditorStyles.boldLabel);
                else if (attributeCurrent is LangAttribute)
                    GUILayout.Label("Lang", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Name");
                    attributeCurrent.Name = EditorGUILayout.TextField(attributeCurrent.Name, GUILayout.MaxWidth(EditorSettings.RegularTextFieldWidth));
                }
                GUILayout.EndHorizontal();
                if (displayDefaultValue)
                {
                    GUILayout.BeginHorizontal();
                    {
                        DrawAttributeValue();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            EditorGUIUtility.labelWidth = oldLabelWidth;
        }
        private void DrawAttributeValue()
        {
            EditorGUILayout.PrefixLabel("Value");
            if (attributeCurrent is BoolAttribute)
            {
                var boolAttribute = attributeCurrent as BoolAttribute;
                boolAttribute.Value = EditorGUILayout.Toggle(boolAttribute.Value, GUILayout.MaxWidth(50));
            }
            else if (attributeCurrent is IntAttribute)
            {
                var intAttribute = attributeCurrent as IntAttribute;
                intAttribute.Value = EditorGUILayout.IntField(intAttribute.Value, GUILayout.MaxWidth(50));
            }
            else if (attributeCurrent is DoubleAttribute)
            {
                var doubleAttribute = attributeCurrent as DoubleAttribute;
                doubleAttribute.Value = EditorGUILayout.DoubleField(doubleAttribute.Value, GUILayout.MaxWidth(50));
            }
            else if (attributeCurrent is StringAttribute)
            {
                var stringAttribute = attributeCurrent as StringAttribute;
                stringAttribute.Value = EditorGUILayout.TextField(stringAttribute.Value, GUILayout.MaxWidth(100));
            }
            else if (attributeCurrent is LangAttribute)
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    if (langAttributeList != null)
                        langAttributeList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
                if (langAttributeCurrent != null)
                    DrawLangAttribute();
            }
        }
        private void DrawLangAttribute()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Value");
                    langAttributeCurrent.Value = EditorGUILayout.TextField(langAttributeCurrent.Value, GUILayout.MaxWidth(EditorSettings.RegularTextFieldWidth));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        public void ClearSelection()
        {
            if (attributeList != null)
                attributeList.ClearSelection();

            if (langAttributeList != null)
                langAttributeList.ClearSelection();

            attributeCurrent = null;
            langAttributeCurrent = null;
        }
    }
}