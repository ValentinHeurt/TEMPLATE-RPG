using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class TableSlot : ItemSlotUI, IDropHandler
{
    [SerializeField] private TextMeshProUGUI itemNameText = null;
    public override Item SlotItem { get; set; }
    private void OnEnable()
    {
        UpdateSlotUI();
        EventManager.Instance.AddListener<OnAmeliorationTableItemUpdated>(OnItemChanged);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

        if (itemDragHandler == null) { return; }

        if ((itemDragHandler.ItemSlotUI as InventorySlot) != null && itemDragHandler.ItemSlotUI.SlotItem.itemType == ItemType.WEAPON || itemDragHandler.ItemSlotUI.SlotItem.itemType == ItemType.EQUIPMENT)
        {
            
            if (SlotItem == null)
            {
                SlotItem = itemDragHandler.ItemSlotUI.SlotItem;
                EventManager.Instance.QueueEvent(new RemoveOneItemGameEvent(itemDragHandler.ItemSlotUI.SlotIndex));
            }
            else
            {
                EventManager.Instance.QueueEvent(new AddOneItemGameEvent(SlotItem));
                SlotItem = itemDragHandler.ItemSlotUI.SlotItem;
                EventManager.Instance.QueueEvent(new RemoveOneItemGameEvent(itemDragHandler.ItemSlotUI.SlotIndex));
            }
        }
        UpdateSlotUI();
    }

    public void HandleTableClosed()
    {
        if (SlotItem != null)
        {
            EventManager.Instance.QueueEvent(new AddOneItemGameEvent(SlotItem));
            SlotItem = null;
            UpdateSlotUI();
        }
    }

    public void OnItemChanged(OnAmeliorationTableItemUpdated eventData)
    {
        SlotItem = eventData.item;
        itemNameText.text = eventData.item.ColoredName;
    }

    public override void UpdateSlotUI()
    {
        EventManager.Instance.QueueEvent(new OnToolTableSlotUpdated(SlotItem));
        if (SlotItem == null)
        {
            EnablesSlotUI(false);
            itemNameText.text = "";
            return;
        }
        EnablesSlotUI(true);
        itemIconImage.sprite = SlotItem.icon;
        itemNameText.text = SlotItem.ColoredName;
    }

    public void RemoveItemFromSlot()
    {
        if(SlotItem != null)
        {
            SlotItem = null;
        }
        UpdateSlotUI();
    }
}
