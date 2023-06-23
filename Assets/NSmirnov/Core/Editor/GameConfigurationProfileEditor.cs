using NSmirnov.Core.Foundation;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace NSmirnov.Core.Editor
{
    [CustomEditor(typeof(GameConfigurationProfile), true)]
    [CanEditMultipleObjects]
    public class GameConfigurationProfileEditor : UnityEditor.Editor
    {
        GameConfigurationProfile profile;

        private void OnEnable()
        {
            profile = (GameConfigurationProfile)target;
        }

        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            foreach (var item in profile.items)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item.TypePath.ToString());
                if (GUILayout.Button("Edit"))
                {
                    var win = ProfileWindowEditor.Init();
                    win.Profiles = profile;
                    win.ProfileType = item.TypePath;
                }
                GUILayout.EndHorizontal();
                if (item.TypePath == ProfileType.Locale && item.IsPersistentDataPath)
                {
                    EditorGUILayout.HelpBox($"Build Path: {Application.persistentDataPath}", MessageType.None, true);
                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox($"Load Path: {Application.persistentDataPath}", MessageType.None, true);
                }
                else if (item.TypePath == ProfileType.Resources)
                {
                    EditorGUILayout.HelpBox($"Build Path: {item?.BuildPath}", MessageType.None, true);
                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox($"Load Path: {item?.LoadPath}", MessageType.None, true);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Build Path: {item?.BuildPath}", MessageType.None, true);
                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox($"Load Path: {item?.LoadPath}", MessageType.None, true);
                }
                GUILayout.Space(5);
            }
            base.serializedObject.ApplyModifiedProperties();
        }
    }
}