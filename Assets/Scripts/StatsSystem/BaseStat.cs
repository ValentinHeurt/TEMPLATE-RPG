using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public Stat stat;
    public List<StatBonus> statAdditives;
    public float baseValue;

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
