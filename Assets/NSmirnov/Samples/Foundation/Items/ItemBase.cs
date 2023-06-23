using NSmirnov.Core.Foundation;
using System;

namespace NSmirnov.Samples.Foundation.Items
{
    [Serializable]
    public abstract class ItemBase : EntryAddressables
    {
        public ItemClass Class;
    }
}
