using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class XpUI : MonoBehaviour
{
    public float currentXp;
    public float currentRequiredXp;
    private float lerpTimer;
    private float frontBarfill;
    private float backBarfill;
    private int currentLevel;
    public Image xpBar;
    public TextMeshProUGUI textLevel;

    private void OnEnable()
    {
        LevelSystem.onXpLvlGained += OnXpLvlChanged;
    }

    private void OnDisable()
    {
        LevelSystem.onXpLvlGained -= OnXpLvlChanged;
    }

    void OnXpLvlChanged(string levelSystemID ,float xp, float requiredXp, int level)
    {
        Debug.Log("xpUI");
        if (levelSystemID == "playerLevel")
        {
            Debug.Log("xpUI entré dans playerLevel");
            currentXp = xp;
            currentRequiredXp = requiredXp;
            textLevel.text = level.ToString();
            xpBar.fillAmount = currentXp / currentRequiredXp;
        }
    }

    //IEnumerator UpdateUI()
    //{
    //    float fillAmount = ;
    //    float hFraction = health / maxHealth;
    //    frontHealthBar.fillAmount = hFraction;
    //    if (backHealthBarfill >= frontHealthBarfill)
    //    {
    //        backHealthBar.color = Color.red;
    //        lerpTimer = Time.deltaTime;
    //        backHealthBar.fillAmount = Mathf.Lerp(backHealthBarfill, hFraction, lerpTimer);
    //    }
    //}



}
