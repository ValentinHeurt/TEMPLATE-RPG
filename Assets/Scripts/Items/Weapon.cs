using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Assets/Weapon")]
public class Weapon : Equipment
{
    [Header("Weapon Fields")]
    [SerializeField] public GameObject weaponPrefab;

    public override string GetInfoDisplayedText()
    {
        StringBuilder builder = new StringBuilder();
        foreach (BaseStat stat in stats)
        {
            builder.Append("<color=green>+").Append(stat.baseValue).Append(" ").Append(stat.statDescription).Append("</color>").AppendLine();
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
}

