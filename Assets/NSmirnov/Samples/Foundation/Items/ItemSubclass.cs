using System;

namespace NSmirnov.Samples.Foundation.Items
{
    [Serializable]
    public abstract class ItemSubclass<T> : ItemBase
    {
        public T Subclass;
    }
}
