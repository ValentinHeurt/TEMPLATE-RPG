using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
public class ToolTableUI : MonoBehaviour
{
    public TextMeshProUGUI itemStats;
    public TextMeshProUGUI statusOfCurrentTry;
    public Button ameliorationButton;
    public Color colorWhenNoItem;
    public Color colorWhenItem;
    private Item item;
    public TableSlot tableSlot;
    private void OnEnable()
    {
        EventManager.Instance.AddListener<OnToolTableSlotUpdated>(ItemSlotUpdated);
    }

    private void ItemSlotUpdated(OnToolTableSlotUpdated eventData)
    {
        item = eventData.item;
        StringBuilder builder = GetStatsStringBuilder(item);
        itemStats.text = builder.ToString();
        UpdateButtonAmeliorationColor();
        EventManager.Instance.QueueEvent(new OnAmeliorationTableItemUpdated(item));
    }

    private StringBuilder GetStatsStringBuilder(Item item)
    {
        StringBuilder builder = new StringBuilder();
        if ((item as Equipment) != null)
        {
            Equipment equipment = item as Weapon;
            foreach (BaseStat stat in equipment.stats)
            {
                builder.Append($"+{stat.baseValue} {stat.stat.StatDescription}").AppendLine();
            }
        }
        else
        {
            builder.Append("");
        }
        return builder;
    }

    private void UpdateButtonAmeliorationColor()
    {
        ColorBlock colorBlock = ameliorationButton.colors;
        colorBlock.normalColor = itemStats.text == "" ? colorWhenNoItem : colorWhenItem;
        colorBlock.highlightedColor = itemStats.text == "" ? colorWhenNoItem : colorWhenItem;
        ameliorationButton.colors = colorBlock; 
    }

    public void CloseTableWindow()
    {
        GameManager.Instance.CursorLocker += -1;
        if (GameManager.Instance.CursorLocker == 0)
            GameManager.Instance.Play();
        else
            GameManager.Instance.Inventory();
        tableSlot.HandleTableClosed();
        gameObject.SetActive(false);
    }

    public void OnClickAmelioration()
    {
        if ((item as Equipment) != null)
        {
            Equipment equipment = item as Equipment;

            // verif si montant requis est match
            if (equipment.ameliorationState < equipment.rarity.maxAmeliorationState)
            {
                //Random
                if (Random.value <= equipment.rarity.upgradeRates[equipment.ameliorationState]/100)
                {
                    // Augmenter les stats
                    foreach (BaseStat stat in equipment.stats)
                    {
                        stat.baseValue = Mathf.Ceil(stat.baseValue + (stat.baseValue * equipment.ameliorationPercent / 100));
                    }
                    equipment.ameliorationState += 1;
                    statusOfCurrentTry.text = "Succes";
                    statusOfCurrentTry.color = Color.green;
                }
                else
                {
                    statusOfCurrentTry.text = "Fail";
                    statusOfCurrentTry.color = Color.gray;
                }
            }
            EventManager.Instance.QueueEvent(new OnToolTableSlotUpdated(equipment));
        }
    }

}
