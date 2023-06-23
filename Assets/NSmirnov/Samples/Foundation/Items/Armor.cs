using NSmirnov.Core;
using System;
using System.ComponentModel;

namespace NSmirnov.Samples.Foundation.Items
{
    [Serializable]
    public class Armor : ItemSubclass<ArmorClass>
    {

        [DisplayName("Защита")]
        public MinMaxAttributes Defence { get; set; } = new MinMaxAttributes();
    }
}
