using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoal : QuestGoal
{
    public Item item;
    public VoidEvent askForInventoryUpdate;
    public override string GetDescription()
    {
        return $"Récupérez un {item}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<OnInventoryUpdate>(OnInventoryUpdate);
        askForInventoryUpdate.Raise();
    }

    private void OnInventoryUpdate(OnInventoryUpdate eventInfo)
    {
        if (questCompleted) return;
        CurrentAmount = eventInfo.inventory.ItemCount(item);
        Evaluate();
    }

    public override void CheckGoal()
    {

    }

}
