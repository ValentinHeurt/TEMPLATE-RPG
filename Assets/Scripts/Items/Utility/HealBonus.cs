using System.Collections;
using System;
using System.Text;
using UnityEngine;
[CreateAssetMenu(fileName = "New Heal Bonus", menuName = "Assets/Bonuses/Heal Bonus")]
public class HealBonus : Bonus
{
    [Header("Heal Bonus")]
    public int value;

    public override bool HandleBonus()
    {
        if (PlayerController.Instance.GetComponent<Damageable>().currentHitPoints >= PlayerController.Instance.GetComponent<Damageable>().maxHitPoints)
        {
            return false;
        }
        EventManager.Instance.QueueEvent(new HealGameEvent(value));
        Debug.Log("test heal " + bonusName);
        return true;
    }
}
