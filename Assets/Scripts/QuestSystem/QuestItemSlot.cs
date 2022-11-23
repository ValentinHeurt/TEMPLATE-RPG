using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestItemSlot : ItemSlotUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    [SerializeField] protected ItemEvent onMouseStartHoverItem = null;
    [SerializeField] protected VoidEvent onMouseEndHoverItem = null;
    public override Item SlotItem
    {
        get { return Itemslot.item; }
        set { }
    }

    public ItemSlot Itemslot;
    public override void OnDrop(PointerEventData eventData)
    {
    }

    private void OnEnable()
    {
        if (SlotItem != null)
            onMouseStartHoverItem.Raise(SlotItem); 
    }

    private void OnDisable()
    {
        onMouseEndHoverItem.Raise();
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseStartHoverItem.Raise(SlotItem);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseEndHoverItem.Raise();
    }
}
