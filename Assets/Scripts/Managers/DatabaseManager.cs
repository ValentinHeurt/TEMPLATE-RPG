using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatabaseManager : Singleton<DatabaseManager>
{
    public ItemDatabase items;

    public static Item GetItemByID(string ID)
    {
        return Instance.items.allItems.FirstOrDefault(x => x.ID == ID);
    }

    public static Weapon GetWeaponByID(string ID)
    {
        return Instance.items.allWeapons.FirstOrDefault(x => x.ID == ID);
    }

}
