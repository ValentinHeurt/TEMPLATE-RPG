﻿using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;
}

public class HealGameEvent : GameEvent
{
    public int amount;

    public HealGameEvent(int healingAmount)
    {
        this.amount = healingAmount;
    }
}

public class RemoveOneItemGameEvent : GameEvent
{
    public int slotId;

    public RemoveOneItemGameEvent(int slotId)
    {
        this.slotId = slotId;
    }
}

public class OnInventoryUpdate : GameEvent
{
    public Inventory inventory;
    public OnInventoryUpdate(Inventory inventory)
    {
        this.inventory = inventory;
    }
}

public class OnQuestSelected : GameEvent
{
    public Quest quest;
    public OnQuestSelected(Quest quest)
    {
        this.quest = quest;
    }
}
public class OnAnswerChosen : GameEvent
{
    public AnswerData answerData;

    public OnAnswerChosen(AnswerData answerData)
    {
        this.answerData = answerData;
    }
}