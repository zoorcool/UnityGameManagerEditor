using NSmirnov.Core.Foundation;
using NSmirnov.Samples.Foundation;
using System.Collections;
using System.Linq;
using UnityEditor;

namespace NSmirnov.Samples.Editor
{
    public class GameEditor : Core.Editor.GameEditorBase<GameConfiguration, GameProperties, GameConfigurationEditor>
    {
        [MenuItem("Window/NSmirnov/Game Configuration")]
        public static void Init()
        {
            var window = GetWindow(typeof(GameEditor));
        }
        protected override void OnEditorInit()
        {
            editors.Add(new WeaponEditor().Init(gameConfig));
            editors.Add(new ArmorEditor().Init(gameConfig));
            editors.Add(new ResourcesEditor().Init(gameConfig));
        }
        protected override void LoadGameConfiguration()
        {
            GameConfigurationProfile.Item profile = profiles.items?.FirstOrDefault(_ => _.TypePath == profileType);

            if (editorCurrent != null)
            {
                editorCurrent.ClearSelection();
                editorCurrent = null;
            }

            editorList.ClearSelection();

            if (profile != null)
            {
                gameConfig.LoadGameConfiguration(profile);
            }

            EditorInit();
        }
        protected override void OnResetGameConfiguration()
        {

        }
    }
}