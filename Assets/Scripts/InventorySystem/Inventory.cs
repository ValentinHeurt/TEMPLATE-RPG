using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Inventory : MonoBehaviour, IItemContainer
{
    [SerializeField] private int size = 20;
    [SerializeField] private UnityEvent onInventoryItemsUpdated = null;
    private ItemSlot[] m_ItemSlots = new ItemSlot[0];
    public ItemSlot GetSlotByIndex(int index) => m_ItemSlots[index];

    private void Start()
    {
        m_ItemSlots = new ItemSlot[20];
    }
    public ItemSlot AddItem(ItemSlot itemSlot)
    {
        for (int i = 0; i < m_ItemSlots.Length; i++)
        {
            if (m_ItemSlots[i].item != null)
            {
                if (m_ItemSlots[i].item == itemSlot.item)
                {
                    int slotRemainingSpace = m_ItemSlots[i].item.maxStack - m_ItemSlots[i].quantity;
                    if (itemSlot.quantity <= slotRemainingSpace)
                    {
                        m_ItemSlots[i].quantity += itemSlot.quantity;
                        itemSlot.quantity = 0;
                        onInventoryItemsUpdated.Invoke();

                        return itemSlot;
                    }
                    else if (slotRemainingSpace > 0)
                    {
                        m_ItemSlots[i].quantity += slotRemainingSpace;
                        itemSlot.quantity -= slotRemainingSpace;
                    }
                }
            }
        }

        for (int i = 0; i < m_ItemSlots.Length; i++)
        {
            if (m_ItemSlots[i].item == null)
            {
                if (itemSlot.quantity <= itemSlot.item.maxStack)
                {
                    m_ItemSlots[i] = itemSlot;
                    itemSlot.quantity = 0;
                    onInventoryItemsUpdated.Invoke();
                    return itemSlot;
                }
                else
                {
                    m_ItemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.maxStack);
                    itemSlot.quantity -= itemSlot.item.maxStack;
                }
            }
        }

        onInventoryItemsUpdated.Invoke();

        return itemSlot;

    }

    public int GetTotalQuantity(Item item)
    {
        int totalCount = 0;

        foreach (ItemSlot itemSlot in m_ItemSlots)
        {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }
            totalCount += itemSlot.quantity;
        }
        return totalCount;
    }

    public bool HasItem(Item item)
    {
        foreach (ItemSlot itemSlot in m_ItemSlots)
        {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }

            return true;
        }
        return false;
    }

    public void RemoveAt(int slotIndex, int quantity = 100000)
    {
        if (slotIndex < 0 || slotIndex > m_ItemSlots.Length - 1) { return; }
        if (m_ItemSlots[slotIndex].quantity > quantity)
        {
            m_ItemSlots[slotIndex].quantity -= quantity;
        }
        else
        {
            m_ItemSlots[slotIndex] = new ItemSlot();
        }
        onInventoryItemsUpdated.Invoke();
    }

    public void RemoveOneItemAt(int slotIndex)
    {
        RemoveAt(slotIndex, 1);
    }

    public void RemoveItem(ItemSlot itemSlot)
    {
        for (int i = 0; i < m_ItemSlots.Length; i++)
        {
            if (m_ItemSlots[i].item != null)
            {
                if (m_ItemSlots[i].item == itemSlot.item)
                {
                    if (m_ItemSlots[i].quantity < itemSlot.quantity)
                    {
                        itemSlot.quantity -= m_ItemSlots[i].quantity;
                        m_ItemSlots[i] = new ItemSlot();
                    }
                    else
                    {
                        m_ItemSlots[i].quantity -= itemSlot.quantity;
                        if (m_ItemSlots[i].quantity == 0)
                        {
                            m_ItemSlots[i] = new ItemSlot();
                            onInventoryItemsUpdated.Invoke();
                            return;
                        }
                    }
                }
            }
        }
    }

    public void Swap(int indexOne, int indexTwo)
    {
        ItemSlot firstSlot = m_ItemSlots[indexOne];
        ItemSlot secondSlot = m_ItemSlots[indexTwo];

        if (firstSlot == secondSlot) { return; }
        if (secondSlot.item != null)
        {
            if (firstSlot.item == secondSlot.item)
            {
                int secondSlotRemainingSpace = secondSlot.item.maxStack - secondSlot.quantity;
                if (firstSlot.quantity <= secondSlotRemainingSpace)
                {
                    m_ItemSlots[indexTwo].quantity += firstSlot.quantity;

                    m_ItemSlots[indexOne] = new ItemSlot();
                    onInventoryItemsUpdated.Invoke();
                    return;
                }

            }
        }

        m_ItemSlots[indexOne] = secondSlot;
        m_ItemSlots[indexTwo] = firstSlot;
        onInventoryItemsUpdated.Invoke();
    }

    public bool AddAt(int index, ItemSlot itemSlot)
    {
        if (m_ItemSlots[index].item == null)
        {
            m_ItemSlots[index] = itemSlot;
            onInventoryItemsUpdated.Invoke();
            return true;
        }
        return false;
    }

}
