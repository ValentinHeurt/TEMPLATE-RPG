using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
[CreateAssetMenu(fileName = "New item", menuName = "Assets/Item/item")]
public class BaseItem : Item
{
    public override string GetInfoDisplayedText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(description);
        return builder.ToString();
    }

    public override void UseItem(ItemSlotUI itemSlotUI)
    {
    }
}