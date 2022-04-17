using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float frontHealthBarfill;
    private float backHealthBarfill;
    public float maxHealth = 100;
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
    public void UpdateHealthUI()
    {
        frontHealthBarfill = frontHealthBar.fillAmount;
        backHealthBarfill = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
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
