using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStateSMB : StateMachineBehaviour
{
    public float minNormTime = 5f;
    public float maxNormTime = 10f;

    protected float m_RandomNormTime;
    readonly int m_HashRandomIdle = Animator.StringToHash("ChangeIdle");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_RandomNormTime = Random.Range(minNormTime, maxNormTime);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > m_RandomNormTime && !animator.IsInTransition(0))
        {
            animator.SetTrigger(m_HashRandomIdle);
        }
        
    }
}


