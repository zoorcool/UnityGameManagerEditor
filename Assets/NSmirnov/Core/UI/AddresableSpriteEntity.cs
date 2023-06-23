using UnityEngine;

namespace NSmirnov.Core.UI
{
    public abstract class AddresableSpriteEntity<T> : MonoBehaviour
    {
        [SerializeField] protected AddresableSprite icon;
        [HideInInspector] public T prop;
        public void SetProp(T prop)
        {
            this.prop = prop;

            Fill();
        }
        protected abstract void Fill();
    }
}