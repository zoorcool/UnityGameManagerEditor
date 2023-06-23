using NSmirnov.Core;
using System;
using System.ComponentModel;

namespace NSmirnov.Samples.Foundation.Items
{
    [Serializable]
    public class Weapon : ItemBase
    {
        [DisplayName("Урон")]
        public MinMaxAttributes Damage { get; set; } = new MinMaxAttributes();
    }
}
