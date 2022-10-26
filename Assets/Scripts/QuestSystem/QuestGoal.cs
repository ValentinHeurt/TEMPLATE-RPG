using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class QuestGoal : ScriptableObject
{
    public string Description;
    public string goalName;
    public int CurrentAmount { get; protected set; }
    public int requiredAmount = 1;
    public bool completed;
    public bool questCompleted = false;

    [HideInInspector] public UnityEvent OnGoalCompleted;

    public virtual string GetDescription()
    {
        return "";
    }

    public virtual void Initialize()
    {
        CurrentAmount = 0;
        completed = false;
        questCompleted = false;
        OnGoalCompleted = new UnityEvent();
    }

    public virtual void CheckGoal()
    {

    }

    protected void Evaluate()
    {
        if (questCompleted) return;
        if(CurrentAmount >= requiredAmount)
        {
            Complete(true);
        }
        else
        {
            Complete(false);
        }
    }

    private void Complete(bool state)
    {
        completed = state;
        OnGoalCompleted.Invoke();
    }
    public void Skip()
    {
        //
    }

}
