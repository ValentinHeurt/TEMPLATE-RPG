using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
[CreateAssetMenu(fileName = "New Consumable", menuName = "Assets/Item/Consumable")]
public class Consumable : Item
{
    [Header("Consumable")]
    public List<Bonus> bonuses;
    public IntEvent onConsumableUsed;
    public void ApplyBonuses()
    {
        foreach (Bonus bonus in bonuses)
        {
            bonus.HandleBonus();
        }
    }

    public override void UseItem(ItemSlotUI itemSlotUI)
    {
        ApplyBonuses();
        EventManager.Instance.QueueEvent(new RemoveOneItemGameEvent(itemSlotUI.SlotIndex));
    }

    public override string GetInfoDisplayedText()
    {
        StringBuilder builder = new StringBuilder();
        foreach (Bonus bonus in bonuses)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(bonus.color);
            builder.Append($"<color=#{hexColor}>+").Append(bonus.valueString.ToString()).Append(" ").Append(bonus.bonusName).Append("</color>").AppendLine();
        }
        return builder.ToString();
    }
}

