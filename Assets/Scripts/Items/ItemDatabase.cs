using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Database", menuName = "Assets/Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> allItems;
    public List<Weapon> allWeapons;
}
