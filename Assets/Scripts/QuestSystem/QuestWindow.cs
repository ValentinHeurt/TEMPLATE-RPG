using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentQuestName;
    [SerializeField] private TextMeshProUGUI currentQuestDescription;
    [SerializeField] private TextMeshProUGUI currentQuestXpReward;
    [SerializeField] private Transform goals;
    [SerializeField] private Transform quests;
    [SerializeField] private Transform rewardsItemHolder;
    [SerializeField] private GoalUIHandler goalPrefab;
    [SerializeField] private QuestUIHandler questPrefab;
    [SerializeField] private GameObject currentQuestUI;
    [SerializeField] private GameObject rewardsItemPrefab;

    private Quest currentQuest;


    public void Initialize(List<Quest> quests)
    {
        if (quests.Count == 0)
        {
            currentQuestUI.SetActive(false);
            return;
        }
        else
        {
            currentQuestUI.SetActive(true);
        }
        foreach(Transform quest in this.quests)
        {
            GameObject.Destroy(quest.gameObject);
        }
        foreach (Quest quest in quests)
        {
            QuestUIHandler tempQuest = Instantiate(questPrefab, this.quests);
            tempQuest.FillInformations(quest.Information.Name, quest);
        }
        if (currentQuest != null)
            EventManager.Instance.QueueEvent(new OnQuestSelected(currentQuest));
        else
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
        currentQuest = eventInfo.quest;
        currentQuestName.text = currentQuest.Information.Name;
        currentQuestDescription.text = currentQuest.Information.Description;
        foreach (Transform goal in goals)
        {
            GameObject.Destroy(goal.gameObject);
        }
        foreach (Transform reward in rewardsItemHolder)
        {
            GameObject.Destroy(reward.gameObject);
        }
        foreach (QuestGoal goal in currentQuest.Goals)
        {
            GoalUIHandler tempGoal = Instantiate(goalPrefab, goals);
            tempGoal.FillInformations(goal.goalName, goal.CurrentAmount.ToString(), goal.requiredAmount.ToString(), goal.completed);
        }
        currentQuestXpReward.text = $"+{currentQuest.rewards.XP} xp";
        currentQuest.rewards.items.ForEach(itemSlot =>
        {
            GameObject itemSlotGO = Instantiate(rewardsItemPrefab, rewardsItemHolder);
            itemSlotGO.GetComponent<QuestItemSlot>().Itemslot = itemSlot;
            itemSlotGO.GetComponent<QuestItemSlot>().UpdateSlotUI();
        });

    }
}
