using System.ComponentModel;
using UnityEditor.PackageManager;

namespace NSmirnov.Samples.Foundation
{
    public enum ItemClass
    {
        [Description("Оружие")]
        Weapon,
        [Description("Броня")]
        Armor,
        [Description("Ресурсы")]
        Resources,
    }
    public enum ArmorClass
    {
        [Description("Common")] Common,
        [Description("Epic")] Epic,
    }
}