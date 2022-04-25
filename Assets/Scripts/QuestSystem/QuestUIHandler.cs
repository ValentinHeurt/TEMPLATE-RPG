using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class QuestUIHandler : MonoBehaviour, IPointerUpHandler
{
    public TextMeshProUGUI questNameText;
    public Quest quest;
    public void FillInformations(string name, Quest quest)
    {
        questNameText.text = name;
        this.quest = quest;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        EventManager.Instance.QueueEvent(new OnQuestSelected(quest));
    }





}
