using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment Part", menuName = "Assets/Equipment Part")]
public class Equipment : Item
{
    [Header("Equipment part")]
    [SerializeField] public List<BaseStat> stats;
    public EquipmentPart equipmentPart;
    public static Action<ItemSlot,EquipmentPart,int> onEquiped;
    public static Action<Equipment> onEquipmentEquiped;
    public static Action<EquipmentPart> onRemoveEquipment;
    public int ameliorationPercent;
    public int ameliorationState = 0;
    public override string GetInfoDisplayedText()
    {
        StringBuilder builder = new StringBuilder();
        foreach (BaseStat stat in stats)
        {
            builder.Append("<color=green>+").Append(stat.baseValue).Append(" ").Append(stat.stat.StatDescription).Append("</color>").AppendLine();
        }
        return builder.ToString();
    }

    public override void UseItem(ItemSlotUI itemSlotUI)
    {
        if ((itemSlotUI as InventorySlot) != null)
        {
            Debug.Log("equipment equiped");
            onEquiped?.Invoke(new ItemSlot(itemSlotUI.SlotItem, 1), equipmentPart, itemSlotUI.SlotIndex);
            onEquipmentEquiped?.Invoke(this);
        }
        if ((itemSlotUI as EquipmentSlot) != null)
        {
            Debug.Log("equipment removed");
            onRemoveEquipment?.Invoke(equipmentPart);
        }
    }

    public override string ColoredName
    {
        get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(rarity.Color);
            if (ameliorationState != 0)
                return $"<color=#{hexColor}>{itemName} +{ameliorationState}</color>";
            else
                return $"<color=#{hexColor}>{itemName}</color>";
        }
    }
}
