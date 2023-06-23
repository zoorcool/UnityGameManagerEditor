using System;

namespace NSmirnov.Core.Foundation
{
    [Serializable]
    public class Lang
    {
        public string Key;
        public string Value;

        public Lang() { }

        public Lang(string Key)
        {
            this.Key = Key;
        }
        public Lang(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
}