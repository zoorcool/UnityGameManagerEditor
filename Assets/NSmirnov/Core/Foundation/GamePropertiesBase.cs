using System.Collections.Generic;

namespace NSmirnov.Core.Foundation
{
    public abstract class GamePropertiesBase : IGameProperties
    {
        public double Money { get; set; }
        public List<string> Langs { get; set; } = new List<string>();
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
    }
}