using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoal : QuestGoal
{
    public Item item;

    public override string GetDescription()
    {
        return $"Récupérez un {item}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<OnGetItem>(OnGetItem);
    }

    private void OnGetItem(OnGetItem eventInfo)
    {
        if (eventInfo.itemSlot.item.itemName == item.itemName)
        {
            CurrentAmount += eventInfo.itemSlot.quantity;
            Evaluate();
        }
    }

}
