using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public int health;
    private float lerpTimer;
    private float frontHealthBarfill;
    private float backHealthBarfill;
    public int maxHealth = 100;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Damageable representedDamageable;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    private void Awake()
    {
        // Listeners
        EventManager.Instance.AddListener<OnStatsUpdated>(HealthStatsUpdate);

    }
    public void HealthStatsUpdate(OnStatsUpdated eventData)
    {
        if (eventData.cible == representedDamageable.gameObject)
        {
            int newMaxHp = (int)eventData.characterStats.GetStat(StatType.HpFlat).GetCalculatedStatValue();
            int dif = newMaxHp - maxHealth;
            if (health / maxHealth == 1f)
            {
                maxHealth = newMaxHp;
                health = newMaxHp;
            }
            else
            {
                maxHealth = newMaxHp;
                health += dif;
            }
        }
    }

    public void UpdateHealthUI()
    {
        frontHealthBarfill = frontHealthBar.fillAmount;
        backHealthBarfill = backHealthBar.fillAmount;
        float hFraction = (float)health / (float)maxHealth;
        frontHealthBar.fillAmount = hFraction;
        if (backHealthBarfill >= frontHealthBarfill)
        {
            backHealthBar.color = Color.red;
            lerpTimer = Time.deltaTime * 5f;
            backHealthBar.fillAmount = Mathf.Lerp(backHealthBarfill, hFraction, lerpTimer);
        }
    }
    public void SetHealth(Damageable damageable)
    {
        this.health = damageable.currentHitPoints;
    }
}
