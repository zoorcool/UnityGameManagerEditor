using UnityEngine;

namespace NSmirnov.Core.UI
{
    public abstract class Entity<T> : MonoBehaviour
    {
        [HideInInspector] public T prop;
        public void SetProp(T prop)
        {
            this.prop = prop;

            Fill();
        }
        protected abstract void Fill();
    }
}