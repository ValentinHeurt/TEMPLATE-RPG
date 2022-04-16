using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class EquipmentSlot : ItemSlotUI, IDropHandler
{
    [SerializeField] private EquipmentHUD equipmentHUD = null;
    [SerializeField] public EquipmentPart equipmentPart;
    public static Action<Equipment> onEquipmentEquiped;
    public override Item SlotItem
    {
        get { return Itemslot.item; }
        set { }
    }
    public ItemSlot Itemslot => equipmentHUD.GetSlotByEquipment(equipmentPart);

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

        if (itemDragHandler == null) { return; }

        if ((itemDragHandler.ItemSlotUI as InventorySlot) != null)
        {
            if ((itemDragHandler.ItemSlotUI.SlotItem as Equipment).equipmentPart == equipmentPart)
            {
                onEquipmentEquiped?.Invoke(itemDragHandler.ItemSlotUI.SlotItem as Equipment);
                equipmentHUD.AddItem((itemDragHandler.ItemSlotUI as InventorySlot).Itemslot, equipmentPart, itemDragHandler.ItemSlotUI.SlotIndex);
            }
        }
    }

    public override void UpdateSlotUI()
    {
        if (Itemslot.item == null)
        {
            EnablesSlotUI(false);
            return;
        }
        EnablesSlotUI(true);
        itemIconImage.sprite = Itemslot.item.icon;
    }
}

public enum EquipmentPart
{
    Weapon,
    Helmet,
    Armor,
    Gauntlets,
    Boots
}