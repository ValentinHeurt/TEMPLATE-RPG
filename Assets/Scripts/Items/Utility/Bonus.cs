using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bonus", menuName = "Assets/Bonuses/Bonus")]
public class Bonus : ScriptableObject
{
    [Header("Bonus")]
    public string bonusName;
    public Color color;
    public string valueString;

    public virtual void HandleBonus()
    {

    }
}