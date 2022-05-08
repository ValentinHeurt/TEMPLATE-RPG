using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentQuestName;
    [SerializeField] private TextMeshProUGUI currentQuestDescription;
    [SerializeField] private Transform goals;
    [SerializeField] private Transform quests;
    [SerializeField] private GoalUIHandler goalPrefab;
    [SerializeField] private QuestUIHandler questPrefab;
    public void Initialize(List<Quest> quests)
    {
        foreach(Transform quest in this.quests)
        {
            GameObject.Destroy(quest.gameObject);
        }
        foreach (Quest quest in quests)
        {
            QuestUIHandler tempQuest = Instantiate(questPrefab, this.quests);
            tempQuest.FillInformations(quest.Information.Name, quest);
        }
        EventManager.Instance.QueueEvent(new OnQuestSelected(quests[0]));
    }

    private void OnEnable()
    {
        Initialize(QuestManager.Instance.currentQuests);
    }

    private void Awake()
    {
        EventManager.Instance.AddListener<OnQuestSelected>(SetDisplayedQuest);
    }
    public void SetDisplayedQuest(OnQuestSelected eventInfo)
    {
        Quest quest = eventInfo.quest;
        currentQuestName.text = quest.Information.Name;
        currentQuestDescription.text = quest.Information.Description;
        foreach (Transform goal in goals)
        {
            GameObject.Destroy(goal.gameObject);
        }
        foreach (QuestGoal goal in quest.Goals)
        {
            GoalUIHandler tempGoal = Instantiate(goalPrefab, goals);
            tempGoal.FillInformations(goal.goalName, goal.CurrentAmount.ToString(), goal.requiredAmount.ToString(), goal.completed);
        }
    }
}
