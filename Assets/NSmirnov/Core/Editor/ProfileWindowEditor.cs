using NSmirnov.Core.Foundation;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NSmirnov.Core.Editor
{
    public class ProfileWindowEditor : EditorWindow
    {
        public ProfileType ProfileType;
        public GameConfigurationProfile Profiles;

        private GameConfigurationProfile.Item profile;

        public static ProfileWindowEditor Init()
        {
            var window = GetWindow(typeof(ProfileWindowEditor));
            window.titleContent = new GUIContent("Profile editor");
            window.minSize = window.maxSize = new Vector2(400, 200);
            window.maximized = false;

            return (ProfileWindowEditor)window;
        }

        private void OnGUI()
        {
            if (Profiles == null) return;

            GUILayout.BeginVertical();
            ProfileType = (ProfileType)EditorGUILayout.EnumPopup("Profile Type:", ProfileType);
            GUILayout.Space(5);

            profile = Profiles.items?.FirstOrDefault(_ => _.TypePath == ProfileType);

            if (profile == null)
            {
                Profiles.items.Add(new GameConfigurationProfile.Item
                {
                    TypePath = ProfileType,
                });

                profile = Profiles.items?.FirstOrDefault(_ => _.TypePath == ProfileType);
            }
            if (ProfileType == ProfileType.Locale && profile.IsPersistentDataPath)
            {
                EditorGUILayout.HelpBox($"Build Path: {Application.persistentDataPath}", MessageType.None, true);
                GUILayout.Space(2);
                EditorGUILayout.HelpBox($"Load Path: {Application.persistentDataPath}", MessageType.None, true);
            }
            else if(ProfileType == ProfileType.Resources)
            {
                string root = "Assets";
                string resources = "Resources";
                string folder = "Config";
                string path = Path.Combine(root, resources);
                if (!AssetDatabase.IsValidFolder(path))
                    AssetDatabase.CreateFolder(root, resources);
                path = Path.Combine(path, folder);
                if (!AssetDatabase.IsValidFolder(path))
                    AssetDatabase.CreateFolder(Path.Combine(root, resources), folder);

                EditorGUILayout.HelpBox($"Build Path: Assets/Resources/Config", MessageType.None, true);
                GUILayout.Space(2);
                EditorGUILayout.HelpBox($"Load Path: Assets/Resources/Config", MessageType.None, true);
            }
            else
            {
                EditorGUILayout.PrefixLabel("Load Path");
                profile.LoadPath = EditorGUILayout.TextField(profile.LoadPath);
                GUILayout.Space(5);
                EditorGUILayout.PrefixLabel("Build Path");
                profile.BuildPath = EditorGUILayout.TextField(profile.BuildPath);
            }
            if (ProfileType == ProfileType.Locale)
            {
                GUILayout.Space(5);
                EditorGUILayout.PrefixLabel("IsPersistentDataPath");
                profile.IsPersistentDataPath = EditorGUILayout.Toggle(profile.IsPersistentDataPath);
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Save"))
            {
                if (profile.TypePath == ProfileType.Resources)
                {
                    profile.LoadPath = profile.BuildPath = "Assets/Resources/Config";
                }

                Profiles.Save();
            }
            GUILayout.EndVertical();
        }
    }
}