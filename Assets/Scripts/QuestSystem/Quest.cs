using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Quest : ScriptableObject
{

    [System.Serializable]
    public struct QuestInformation
    {
        public string Name;
        public Sprite Icon;
        public string Description;
    }
    [Header("Info")] public QuestInformation Information;

    [System.Serializable]
    public struct QuestStat
    {
        public int Currency;
        public int XP;
    }

    [Header("Info")] public QuestStat reward = new QuestStat { Currency = 10, XP = 10 };


    public List<QuestGoal> Goals;
    public bool completed;
    public QuestCompletedEvent OnQuestCompleted;

    public virtual void Initialize()
    {
        completed = false;
        OnQuestCompleted = new QuestCompletedEvent();

        foreach (var goal in Goals)
        {
            goal.Initialize();
            goal.OnGoalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }

    private void CheckGoals()
    {
        completed = Goals.All(goal => goal.completed);
        if (completed)
        {
            // donner reward
            OnQuestCompleted.Invoke(this);
            OnQuestCompleted.RemoveAllListeners();
            foreach (var goal in Goals)
            {
                goal.OnGoalCompleted.RemoveAllListeners();
            }
        }
    }

}

public class QuestCompletedEvent : UnityEvent<Quest> { }