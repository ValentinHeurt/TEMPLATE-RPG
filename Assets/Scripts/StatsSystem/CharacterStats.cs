using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public List<BaseStat> stats = new List<BaseStat>();

    public CharacterStats(float atkFlat, float atkPercent, float critRate, float critDmg)
    {
        stats = new List<BaseStat>()
        {
            new BaseStat(BaseStat.StatType.AtkFlat, atkFlat, "Attaque"),
            new BaseStat(BaseStat.StatType.AtkPercent, atkPercent, "Attaque %"),
            new BaseStat(BaseStat.StatType.CritRate, critRate, "Taux crit"),
            new BaseStat(BaseStat.StatType.CritDmg, critDmg, "Dommages critiques")
        };
    }

    public BaseStat GetStat(BaseStat.StatType statType)
    {
        return this.stats.Find(x => x.statType == statType);
    }

    public void AddStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat statBonus in statBonuses)
        {
            GetStat(statBonus.statType).AddStatBonus(new StatBonus(statBonus.baseValue));
        }
    }

    public void RemoveStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat statBonus in statBonuses)
        {
            GetStat(statBonus.statType).RemoveStatBonus(new StatBonus(statBonus.baseValue));
        }
    }
}
