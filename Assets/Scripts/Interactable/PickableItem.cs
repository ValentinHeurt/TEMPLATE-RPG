using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PickableItem : Interactable
{
    public static Action<ItemSlot> OnItemPicked;
    public ItemSlot itemSlot;
    public override string ColoredName { get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(itemSlot.item. rarity.Color);
            return $"<color=#{hexColor}>{interactableName}</color>";
        } 
    }
    public override void PlayerInteracted(GameObject interacted)
    {
        if (interacted.GetComponent<Inventory>() != null)
        {
            interacted.GetComponent<Inventory>().AddItem(itemSlot);
        }
        
        Destroy(gameObject);
    }
}
