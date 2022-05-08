using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class QuestUIHandler : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI questNameText;
    public Quest quest;
    public void FillInformations(string name, Quest quest)
    {
        questNameText.text = name;
        if (quest.completed)
        {
            questNameText.text = $"<s>{name}</s>";
        }
        this.quest = quest;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventManager.Instance.QueueEvent(new OnQuestSelected(quest));
        }
    }


}
