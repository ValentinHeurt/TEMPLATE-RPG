using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MetierUsageUIHandler : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI lvlReq;
    public Image itemIcon;
    public void FillInformations(string name, string lvl, Sprite icon)
    {
        itemNameText.text = name;
        itemIcon.sprite = icon;
        lvlReq.text = $"Lvl {lvl}";
    }
}
