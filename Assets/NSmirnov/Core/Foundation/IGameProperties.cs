using System.Collections.Generic;

namespace NSmirnov.Core.Foundation
{
    public interface IGameProperties
    {
        double Money { get; set; }
        List<string> Langs { get; set; }
        List<Attribute> Attributes { get; set; }
    }
}