#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace NSmirnov.Core.Foundation
{
    public class BaseScriptable : ScriptableObject, ISerializationCallbackReceiver
    {
        public override string ToString()
        {
            return name + "; id: " + GetInstanceID();
        }

        public void Save()
        {
#if UNITY_EDITOR
            SetAsDirty();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("-------------------Save asset: " + ToString() + " ----------------------------------------------");
#endif
        }

        public void SetAsDirty()
        {
#if UNITY_EDITOR
            if (this)
            {
                EditorUtility.SetDirty(this);
                Debug.Log("-------------------Set dirty: " + ToString() + " ----------------------------------------------");
            }
#endif
        }
        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}