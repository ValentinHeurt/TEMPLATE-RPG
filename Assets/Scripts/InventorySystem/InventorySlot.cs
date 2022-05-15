using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : ItemSlotUI, IDropHandler
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    [SerializeField] private EquipmentHUD equipmentHUD = null;
    public static Action<EquipmentPart> onRemoveEquipment;
    public override Item SlotItem
    {
        get { return Itemslot.item; }
        set { }
    }
    public ItemSlot Itemslot => inventory.GetSlotByIndex(SlotIndex);

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

        if (itemDragHandler == null) { return; }

        if ((itemDragHandler.ItemSlotUI as InventorySlot) != null)
        {
            inventory.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
        }
        if ((itemDragHandler.ItemSlotUI as EquipmentSlot) != null)
        {
            bool succes = inventory.AddAt(SlotIndex, (itemDragHandler.ItemSlotUI as EquipmentSlot).Itemslot);
            if (succes)
            {
                onRemoveEquipment?.Invoke((itemDragHandler.ItemSlotUI as EquipmentSlot).equipmentPart);
                equipmentHUD.RemoveItem((itemDragHandler.ItemSlotUI as EquipmentSlot).equipmentPart);
            }
        }
        if ((itemDragHandler.ItemSlotUI as TableSlot) != null)
        {
            if (itemDragHandler.ItemSlotUI.SlotItem != null)
            {
                inventory.AddItem(new ItemSlot(itemDragHandler.ItemSlotUI.SlotItem,1));
                (itemDragHandler.ItemSlotUI as TableSlot).RemoveItemFromSlot();
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
        itemQuantityText.text = Itemslot.quantity > 1 ? Itemslot.quantity.ToString() : "";
    }

    protected override void EnablesSlotUI(bool enable)
    {
        base.EnablesSlotUI(enable);
        itemQuantityText.enabled = enable;
    }


}
