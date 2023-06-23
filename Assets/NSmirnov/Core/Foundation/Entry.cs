using System;
using System.Collections.Generic;

namespace NSmirnov.Core.Foundation
{
    [Serializable]
    public abstract class Entry
    {
        public string Id;
        public int LV;
        public List<Lang> Name = new List<Lang>();
        public List<Lang> Description = new List<Lang>();
    }
}