using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ItemSlot
{
    public Item item;
    public int quantity;

    public ItemSlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
    public static bool operator == (ItemSlot a, ItemSlot b) { return a.Equals(b); }
    public static bool operator != (ItemSlot a, ItemSlot b) { return !a.Equals(b); }
}
