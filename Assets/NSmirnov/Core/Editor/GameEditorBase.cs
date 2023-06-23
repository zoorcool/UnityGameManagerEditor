using NSmirnov.Core.Foundation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NSmirnov.Core.Editor
{
    public abstract class GameEditorBase<C, P, E> : EditorWindow where C : IGameConfiguration<P>, new() where P : IGameProperties, new() where E : IBaseEditor<C, P>, new()
    {
        const float k_DefaultHorizontalSplitterRatio = 0.33f;
        const float k_MinProfilePaneWidth = 0.10f;
        const float k_MaxProfilePaneWidth = 0.6f;
        const int k_SplitterThickness = 2;
        const int k_ToolbarHeight = 20;
        const int k_ItemRectPadding = 15;

        protected GUIStyle m_ItemRectPadding;

        protected bool m_IsResizingHorizontalSplitter;
        protected float m_HorizontalSplitterRatio = k_DefaultHorizontalSplitterRatio;
        protected Vector2 m_ProfilesPaneScrollPosition;
        protected Vector2 m_VariablesPaneScrollPosition;

        protected C gameConfig;
        protected ProfileType profileType;
        protected GameConfigurationProfile profiles;

        protected List<IBaseEditor<C, P>> editors = new List<IBaseEditor<C, P>>();
        protected ReorderableList editorList;
        protected IBaseEditor<C, P> editorCurrent;
        private void OnEnable()
        {
            titleContent = new GUIContent("Game Configuration");

            m_ItemRectPadding = new GUIStyle();
            m_ItemRectPadding.padding = new RectOffset(k_ItemRectPadding, k_ItemRectPadding, k_ItemRectPadding, k_ItemRectPadding);

            gameConfig = new C();
            LoadProfile();
            EditorInit();
        }
        private void CreateProfile()
        {
            string root = "Assets/NSmirnov";
            string resources = "Resources";
            string data = "Data";
            string path = Path.Combine(root, resources);
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(root, resources);
            path = Path.Combine(path, data);
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(Path.Combine(root, resources), data);

            GameConfigurationProfile asset = ScriptableObject.CreateInstance<GameConfigurationProfile>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, "GameConfigurationProfile.asset"));
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

            profiles = (GameConfigurationProfile)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(GameConfigurationProfile));
        }
        private void LoadProfile()
        {
            profiles = (GameConfigurationProfile)AssetDatabase.LoadAssetAtPath("Assets/NSmirnov/Resources/Data/GameConfigurationProfile.asset", typeof(GameConfigurationProfile));
        }
        protected void EditorInit()
        {
            editors.Clear();
            editors.Add(new E().Init(gameConfig));
            OnEditorInit();

            editorList = EditorUtils.SetupReorderableList("", editors, (rect, x) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight), x.Title);
            },
            (x) =>
            {
                if (editorCurrent != null)
                    editorCurrent.ClearSelection();

                editorCurrent = x;
            },
            () =>
            {

            },
            (x) =>
            {
                editorCurrent = null;
            }, false, false, false, false);
        }
        protected abstract void OnEditorInit();
        private void OnGUI()
        {
            if (profiles == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                EditorGUILayout.HelpBox("Profile not found", MessageType.Warning, true);
                GUILayout.Space(5);
                if (GUILayout.Button("Create Profile", GUILayout.MaxWidth(128)))
                    CreateProfile();
                GUILayout.EndVertical();
                GUILayout.Space(5);
                GUILayout.EndHorizontal();
                return;
            }

            if (m_IsResizingHorizontalSplitter)
                m_HorizontalSplitterRatio = Mathf.Clamp(Event.current.mousePosition.x / position.width,
                    k_MinProfilePaneWidth, k_MaxProfilePaneWidth);

            var toolbarRect = new Rect(0, 0, position.width, position.height);
            var profilesPaneRect = new Rect(0, k_ToolbarHeight, (position.width * m_HorizontalSplitterRatio), position.height);
            var variablesPaneRect = new Rect(profilesPaneRect.width + k_SplitterThickness, k_ToolbarHeight,
                position.width - profilesPaneRect.width - k_SplitterThickness, position.height - k_ToolbarHeight);
            var horizontalSplitterRect = new Rect(profilesPaneRect.width, k_ToolbarHeight, k_SplitterThickness, position.height - k_ToolbarHeight);

            ProfilePanel(profilesPaneRect);
            VariablesPane(variablesPaneRect);
        }
        private void ProfilePanel(Rect profilesPaneRect)
        {
            DrawOutline(profilesPaneRect, 1);

            GUILayout.BeginArea(profilesPaneRect);
            {
                m_ProfilesPaneScrollPosition = GUILayout.BeginScrollView(m_ProfilesPaneScrollPosition);
                {
                    GameConfigurationProfile.Item profile = profiles.items?.FirstOrDefault(_ => _.TypePath == profileType);

                    if (profile == null)
                    {
                        profiles.items.Add(new GameConfigurationProfile.Item
                        {
                            TypePath = profileType
                        });

                        profile = profiles.items.FirstOrDefault(_ => _.TypePath == profileType);
                    }

                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                        {
                            profileType = (ProfileType)EditorGUILayout.EnumPopup("Save & Load Paths:", profileType);
                            GUILayout.Space(2);
                            if (GUILayout.Button("Edit"))
                            {
                                var win = ProfileWindowEditor.Init();
                                win.Profiles = profiles;
                                win.ProfileType = profileType;
                            }
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                        EditorGUILayout.HelpBox($"Build Path: {profile?.BuildPath}", MessageType.None, true);
                        GUILayout.Space(2);
                        EditorGUILayout.HelpBox($"Load Path: {profile?.LoadPath}", MessageType.None, true);
                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("New"))
                                ResetGameConfiguration();
                            GUILayout.Space(2);
                            if (GUILayout.Button("Load"))
                                LoadGameConfiguration();
                            GUILayout.Space(2);
                            if (GUILayout.Button("Save"))
                                gameConfig.SaveGameConfiguration(profile);
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(2);
                            if (editorList != null)
                                editorList.DoLayoutList();
                            GUILayout.Space(2);
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        public static void DrawOutline(Rect rect, float size)
        {
            Color color = new Color(0.6f, 0.6f, 0.6f, 1.333f);
            if (EditorGUIUtility.isProSkin)
            {
                color.r = 0.12f;
                color.g = 0.12f;
                color.b = 0.12f;
            }

            if (Event.current.type != EventType.Repaint)
                return;

            Color orgColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = UnityEngine.GUI.color * color;
            UnityEngine.GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, size), EditorGUIUtility.whiteTexture);
            UnityEngine.GUI.DrawTexture(new Rect(rect.x, rect.yMax - size, rect.width, size), EditorGUIUtility.whiteTexture);
            UnityEngine.GUI.DrawTexture(new Rect(rect.x, rect.y + 1, size, rect.height - 2 * size), EditorGUIUtility.whiteTexture);
            UnityEngine.GUI.DrawTexture(new Rect(rect.xMax - size, rect.y + 1, size, rect.height - 2 * size), EditorGUIUtility.whiteTexture);

            UnityEngine.GUI.color = orgColor;
        }
        protected abstract void OnResetGameConfiguration();
        protected void ResetGameConfiguration()
        {
            if (editorCurrent != null)
            {
                editorCurrent.ClearSelection();
                editorCurrent = null;
            }

            editorList.ClearSelection();

            gameConfig = new C();

            EditorInit();

            OnResetGameConfiguration();
        }
        protected abstract void LoadGameConfiguration();
        private void VariablesPane(Rect variablesPaneRect)
        {
            DrawOutline(variablesPaneRect, 1);
            GUILayout.BeginArea(variablesPaneRect);
            {
                m_VariablesPaneScrollPosition = GUILayout.BeginScrollView(m_VariablesPaneScrollPosition);
                {
                    if (editorCurrent != null)
                        editorCurrent.Draw();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
    }
}