using System.ComponentModel;
using UnityEditor.PackageManager;

namespace NSmirnov.Samples.Foundation
{
    public enum ItemClass
    {
        [Description("������")]
        Weapon,
        [Description("�����")]
        Armor,
        [Description("�������")]
        Resources,
    }
    public enum ArmorClass
    {
        [Description("Common")] Common,
        [Description("Epic")] Epic,
    }
}