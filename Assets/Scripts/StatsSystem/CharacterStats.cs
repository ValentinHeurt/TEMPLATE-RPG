using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    public List<BaseStat> stats = new List<BaseStat>();

    public BaseStat GetStat(StatType statName)
    {
        return this.stats.Find(x => x.stat.StatName == statName);
    }

    public void AddStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat statBonus in statBonuses)
        {
            GetStat(statBonus.stat.StatName).AddStatBonus(new StatBonus(statBonus.baseValue));
        }
    }

    public void RemoveStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat statBonus in statBonuses)
        {
            GetStat(statBonus.stat.StatName).RemoveStatBonus(new StatBonus(statBonus.baseValue));
        }
    }
}
