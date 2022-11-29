using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "New Quest", menuName = "Assets/Quest")]
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
        public List<ItemSlot> items;
    }

    [Header("Info")] public QuestStat rewards = new QuestStat { Currency = 10, XP = 10 };
    
    public List<QuestGoal> Goals;
    public bool completed;

    [Header("Completed Event")]
    public QuestEvent OnQuestCompleted;
    public bool canComplete;

    public virtual void Initialize()
    {
        completed = false;
        canComplete = false;
        foreach (var goal in Goals)
        {
            goal.Initialize();
            goal.OnGoalCompleted.AddListener(delegate { CheckGoals(); });
        }
        HandleItems();
    }

    public void CheckGoals()
    {
        canComplete = Goals.All(goal => goal.completed);
    }

    public void FinishQuest()
    {
        if (canComplete)
        {
            completed = true;
            // donner reward
            OnQuestCompleted.Raise(this);
            foreach (var goal in Goals)
            {
                goal.OnGoalCompleted.RemoveAllListeners();
                goal.questCompleted = true;
            }
        }
    }

    private void HandleItems()
    {
        rewards.items.ForEach(item =>
        {
            item = new ItemSlot(Instantiate(item.item),item.quantity);
        });
    }

}