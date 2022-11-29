using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public abstract class Item : ScriptableObject
{
    [Header("Item Fields")]
    public string ID;
    public ItemType itemType;
    [SerializeField] public Rarity rarity = null;
    [TextArea] public string description;
    public string itemName;
    [Min(1)]public int maxStack;
    public Sprite icon;
    public virtual string ColoredName {
        get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(rarity.Color);
            return $"<color=#{hexColor}>{itemName}</color>";
        }
    }
    public abstract string GetInfoDisplayedText();

    public abstract void UseItem(ItemSlotUI itemSlotUI);

}

public enum ItemType
{
    WEAPON,
    EQUIPMENT,
    CONSUMABLE,
    DIVERS
}