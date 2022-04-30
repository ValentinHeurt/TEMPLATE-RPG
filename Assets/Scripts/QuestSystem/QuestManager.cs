using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private GameObject questHolder;

    public List<Quest> currentQuests;
    public Quest followedQuest;

    public override void Awake()
    {
        base.Awake();
        if (followedQuest is null && currentQuests.Count > 0)
        {
            FollowQuest(currentQuests[0]);
        }
    }

    public void AddQuest(Quest quest)
    {
        if (!currentQuests.Contains(quest))
        {
            quest.Initialize();
            quest.OnQuestCompleted.AddListener(HandleReward);
            currentQuests.Add(quest);
        }
    }

    public void FollowQuest(Quest quest)
    {
        followedQuest = quest;
        questHolder.SetActive(true);
        questHolder.transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text = quest.Information.Name;
        questHolder.transform.Find("QuestGoal").GetComponent<TextMeshProUGUI>().text = $"{quest.Goals[0].CurrentAmount} / {quest.Goals[0].requiredAmount}";
    }

    public void StopFollowingQuest()
    {
        followedQuest = null;
        questHolder.SetActive(false);
    }

    public void HandleReward(Quest quest)
    {
        PlayerController.Instance.GainExperience(quest.reward.XP);
    }

}
