using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Assets/Item/Item")]
public abstract class Item : ScriptableObject
{
    [Header("Item Fields")]
    public string ID;
    public string itemType;
    [SerializeField] public Rarity rarity = null;
    [TextArea] public string description;
    public string itemName;
    [Min(1)]public int maxStack;
    public Sprite icon;
    public string ColoredName {
        get
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(rarity.Color);
            return $"<color=#{hexColor}>{itemName}</color>";
        }
    }
    public abstract string GetInfoDisplayedText();

    public abstract void UseItem(ItemSlotUI itemSlotUI);
}