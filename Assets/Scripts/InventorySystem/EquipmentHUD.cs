using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

public class EquipmentHUD : MonoBehaviour
{
    [SerializeField] private int size = 20;
    [SerializeField] private UnityEvent onInventoryItemsUpdated = null;
    private Inventory inventory;


    public ItemSlot weapon;
    public ItemSlot helmet;
    public ItemSlot armor;
    public ItemSlot boots;
    public ItemSlot gauntlets;
    private Dictionary<EquipmentPart, ItemSlot> m_ItemSlots = new Dictionary<EquipmentPart, ItemSlot>();

    private void OnEnable()
    {
        Equipment.onEquiped += AddItemNoReturn;
        Equipment.onRemoveEquipment += TransfertItemToInventory;
    }
    private void OnDisable()
    {
        Equipment.onEquiped -= AddItemNoReturn;
        Equipment.onRemoveEquipment -= TransfertItemToInventory;
    }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        m_ItemSlots.Add(EquipmentPart.Weapon, weapon);
        m_ItemSlots.Add(EquipmentPart.Helmet, helmet);
        m_ItemSlots.Add(EquipmentPart.Armor, armor);
        m_ItemSlots.Add(EquipmentPart.Boots, boots);
        m_ItemSlots.Add(EquipmentPart.Gauntlets, gauntlets);
    }

    public void AddItemNoReturn(ItemSlot itemSlot, EquipmentPart equipmentPart, int slotIndex = -1)
    {
        AddItem(itemSlot, equipmentPart, slotIndex);
    }

    public ItemSlot AddItem(ItemSlot itemSlot, EquipmentPart equipmentPart, int slotIndex = -1)
    {
        ItemSlot item = m_ItemSlots[equipmentPart];
        if (item.item != null)
        {
            if (slotIndex != -1)
            {
                inventory.RemoveAt(slotIndex, 1);
                inventory.AddItem(item);
                item = itemSlot;
                m_ItemSlots[equipmentPart] = new ItemSlot(item.item, 1);
            }
            onInventoryItemsUpdated.Invoke();
            return item;
        }

        item = itemSlot;

        m_ItemSlots[equipmentPart] = new ItemSlot(item.item, 1);

        inventory.RemoveAt(slotIndex, 1);
        onInventoryItemsUpdated.Invoke();

        return item;

    }

    public bool HasItem(Item item)
    {
        ItemSlot[] itemSlots = m_ItemSlots.Values.ToArray();
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }

            return true;
        }
        return false;
    }

    public void RemoveItem(EquipmentPart equipmentPart)
    {
        m_ItemSlots[equipmentPart] = new ItemSlot();
        onInventoryItemsUpdated.Invoke();
    }

    public ItemSlot GetSlotByEquipment(EquipmentPart equipmentPart)
    {
        return m_ItemSlots[equipmentPart];
    }

    public void TransfertItemToInventory(EquipmentPart equipmentPart)
    {
        ItemSlot tempItemSlot = m_ItemSlots[equipmentPart];
        m_ItemSlots[equipmentPart] = new ItemSlot();
        inventory.AddItem(tempItemSlot);
    }

}
