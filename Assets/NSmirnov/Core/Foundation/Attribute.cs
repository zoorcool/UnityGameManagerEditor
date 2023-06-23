using System;
using System.Collections.Generic;
using System.Linq;

namespace NSmirnov.Core.Foundation
{
    [Serializable]
    public class Attribute
    {
        public string Name;
    }
    [Serializable]
    public class BoolAttribute : Attribute
    {
        public bool Value;
    }
    [Serializable]
    public class IntAttribute : Attribute
    {
        public int Value;
    }
    [Serializable]
    public class DoubleAttribute : Attribute
    {
        public double Value;
    }
    [Serializable]
    public class StringAttribute : Attribute
    {
        public string Value;
    }
    [Serializable]
    public class LangAttribute : Attribute
    {
        public List<Lang> Value = new List<Lang>();
        public string NameByLang(string key) => Value.FirstOrDefault(_ => _.Key == key)?.Value;
    }
}