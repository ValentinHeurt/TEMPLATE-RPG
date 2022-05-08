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
        EventManager.Instance.AddListener<OnInventoryUpdate>(OnInventoryUpdate);
    }

    private void OnInventoryUpdate(OnInventoryUpdate eventInfo)
    {
        if (questCompleted) return;
        CurrentAmount = eventInfo.inventory.ItemCount(item);
        Evaluate();
    }

}
