using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Stat", menuName = "Assets/Stat")]
public class Stat : ScriptableObject
{
    [Header("Stat Fields")]
    public StatType StatName;
    public string StatDescription;
}

public enum StatType { AtkFlat, AtkPercent, CritRate, CritDmg }
