using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{

    public enum StatType { AtkFlat, AtkPercent, CritRate, CritDmg }

    public StatType statType;
    public List<StatBonus> statAdditives;
    public float baseValue;
    public string statDescription;

    public BaseStat(StatType statType, float baseValue, string statDescription)
    {
        statAdditives = new List<StatBonus>();
        this.baseValue = baseValue;
        this.statDescription = statDescription;
        this.statType = statType;
    }

    public void AddStatBonus(StatBonus statbonus)
    {
        statAdditives.Add(statbonus);
    }
    public void RemoveStatBonus(StatBonus statBonus)
    {
        statAdditives.Remove(statAdditives.Find(x=> x.BonusValue == statBonus.BonusValue));
    }

    public float GetCalculatedStatValue()
    {
        float finalValue = 0;
        statAdditives.ForEach(x => finalValue += x.BonusValue);
        finalValue += baseValue;
        return finalValue;
    }
}
