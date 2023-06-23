using NSmirnov.Core.Foundation;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEngine;

namespace NSmirnov.Core.Editor
{
    public abstract class BaseEditor<C, P> : IBaseEditor<C, P> where C : IGameConfiguration<P>, new() where P : IGameProperties, new()
    {
        protected C gameConfig;
        protected string title = "Editor item";
        public string Title => title;
        public IBaseEditor<C, P> Init(C gameConfig)
        {
            this.gameConfig = gameConfig;

            OnInit();

            return this;
        }
        protected abstract void OnInit();
        public void Draw()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        OnDraw();
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
        protected abstract void OnDraw();
        public abstract void ClearSelection();
        protected void DrawAddressableAssetEntryPreview(AddressableAssetEntry entry, int width = 24, int height = 24)
        {
            Texture2D icon = EditorUtils.GetAddressableAssetEntryPreview(entry);

            if (icon)
            {
                GUILayout.Box(icon, GUILayout.Height(height), GUILayout.Width(width));
            }
        }
        protected void DrawAddressableAssetEntryPreview(Rect rect, AddressableAssetEntry entry)
        {
            Texture2D icon = EditorUtils.GetAddressableAssetEntryPreview(entry);

            if (icon)
            {
                EditorGUI.DrawPreviewTexture(rect, icon);
            }
        }
    }
}