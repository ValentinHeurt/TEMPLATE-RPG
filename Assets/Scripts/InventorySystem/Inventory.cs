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


    private void Awake()
    {
        EventManager.Instance.AddListener<RemoveOneItemGameEvent>(RemoveOneItemAt);
        EventManager.Instance.AddListener<AddOneItemGameEvent>(AddOneItem);
    }

    private void Start()
    {
        m_ItemSlots = new ItemSlot[20];
    }
    public void AddItem(ItemSlot itemSlot)
    {
        for (int i = 0; i < m_ItemSlots.Length; i++)
        {
            if (m_ItemSlots[i].item != null)
            {
                if (m_ItemSlots[i].item.ID == itemSlot.item.ID)
                {
                    int slotRemainingSpace = m_ItemSlots[i].item.maxStack - m_ItemSlots[i].quantity;
                    if (itemSlot.quantity <= slotRemainingSpace)
                    {
                        m_ItemSlots[i].quantity += itemSlot.quantity;
                        itemSlot.quantity = 0;
                        onInventoryItemsUpdated.Invoke();
                        EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
                        return;
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
                    EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
                    itemSlot.quantity = 0;
                    onInventoryItemsUpdated.Invoke();
                    return;
                }
                else
                {
                    m_ItemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.maxStack);
                    EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
                    itemSlot.quantity -= itemSlot.item.maxStack;
                }
            }
        }
        onInventoryItemsUpdated.Invoke();

        return;

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

    public int ItemCount(Item item)
    {
        int count = 0;
        foreach (ItemSlot itemSlot in m_ItemSlots)
        {
            if (itemSlot.item == null || itemSlot.item.ID != item.ID) { continue; }
            count += itemSlot.quantity;
        }
        return count;
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
        EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
        onInventoryItemsUpdated.Invoke();
    }

    public void RemoveOneItemAt(RemoveOneItemGameEvent eventData)
    {
        RemoveAt(eventData.slotId, 1);
        EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
    }

    public void AddOneItem(AddOneItemGameEvent eventData)
    {
        AddItem(new ItemSlot(eventData.item, 1));
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
                            EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
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
            EventManager.Instance.QueueEvent(new OnInventoryUpdate(this));
            return true;
        }
        return false;
    }

}
