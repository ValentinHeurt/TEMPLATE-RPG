using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questsContent;
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
            currentQuests.Add(quest);
        }
    }

    public void FollowQuest(Quest quest)
    {
        followedQuest = quest;
    }

}
