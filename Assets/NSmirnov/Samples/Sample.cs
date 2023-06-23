using NSmirnov.Core;
using NSmirnov.Core.UI;
using NSmirnov.Samples.Foundation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sample : Singleton<Sample>
{
    public GameManager gameManager => GameManager.Instance;

    [SerializeField] private AddresableSprite prefab;
    [SerializeField] private ScrollRect scroll;
    private void Start()
    {
        LoadItems(ItemClass.Weapon);
    }
    public void LoadItems(ItemClass @class)
    {
        foreach (Transform t in scroll.content)
            Destroy(t.gameObject);

        switch(@class)
        {
            case ItemClass.Weapon:
                foreach(var prop in gameManager.Config.Weapons)
                {
                    AddresableSprite item = Instantiate(prefab, scroll.content, false);
                    item.SetGuid(prop.EntryGuid);
                }
                break;
            case ItemClass.Resources:
                foreach (var prop in gameManager.Config.Resources)
                {
                    AddresableSprite item = Instantiate(prefab, scroll.content, false);
                    item.SetGuid(prop.EntryGuid);
                }
                break;
            case ItemClass.Armor:
                foreach (var prop in gameManager.Config.Armors)
                {
                    AddresableSprite item = Instantiate(prefab, scroll.content, false);
                    item.SetGuid(prop.EntryGuid);
                }
                break;
        }
    }
}
