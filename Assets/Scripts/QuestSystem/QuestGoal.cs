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

    [HideInInspector] public UnityEvent OnGoalCompleted;

    public virtual string GetDescription()
    {
        return "";
    }

    public virtual void Initialize()
    {
        completed = false;
        OnGoalCompleted = new UnityEvent();
    }

    protected void Evaluate()
    {
        if(CurrentAmount >= requiredAmount)
        {
            Complete();
        }
    }

    private void Complete()
    {
        completed = true;
        OnGoalCompleted.Invoke();
        //removeall ??
    }
    public void Skip()
    {
        //
    }

}
