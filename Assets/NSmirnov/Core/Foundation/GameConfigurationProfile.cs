using System;
using System.Collections.Generic;
using UnityEngine;

namespace NSmirnov.Core.Foundation
{
    public class GameConfigurationProfile : BaseScriptable
    {
        [Serializable]
        public class Item
        {
            public ProfileType TypePath;
            public string BuildPath;
            public string LoadPath;
            public bool IsPersistentDataPath;
        }

        public List<Item> items = new List<Item>();
    }


}