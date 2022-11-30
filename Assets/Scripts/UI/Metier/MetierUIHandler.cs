using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MetierUIHandler : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI metierNameText;
    public Image metierIcon;
    private Metier _metier;

    //XP BAR
    public float currentXp;
    public float currentRequiredXp;
    public Image xpBar;
    public TextMeshProUGUI textLevel;

    void FillBar()
    {
        Debug.Log("xpUI");
        currentXp = _metier.levelSystem.currentXp;
        currentRequiredXp = _metier.levelSystem.requiredXp;
        textLevel.text = _metier.levelSystem.currentLevel.ToString();
        xpBar.fillAmount = currentXp / currentRequiredXp;
    }

    public void FillInformations(Metier metier)
    {
        metierNameText.text = metier.name;
        metierIcon.sprite = metier.icon;
        this._metier = metier;
        FillBar();
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventManager.Instance.QueueEvent(new OnMetierSelected(_metier));
        }
    }
}
