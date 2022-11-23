using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainer
{
    void AddItem(ItemSlot itemSlot);
    void RemoveItem(ItemSlot itemSlot);
    void RemoveAt(int slotIndex,int quantity);
    void Swap(int indexOne, int indexTwo);
    bool HasItem(Item item);
    int GetTotalQuantity(Item item);
}
