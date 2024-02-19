using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] //이게 붙어있으면, 해당 요소는 제거하지 못함

public class BossAnimationReceiver : MonoBehaviour
{
    public System.Action onFinished;
    public System.Action onAttackRight;
    public System.Action onAttackLeft;
    public System.Action onAttackFront;
    public System.Action onSummon;
    
    public void OnFinished()
    {
        if(this.onFinished != null)
        {
            this.onFinished();
        }
    }
    public void OnAttackRight()
    {
        if (this.onAttackRight != null)
        {
            this.onAttackRight();
        }
    }

    public void OnAttackLeft()
    {
        if (this.onAttackLeft != null)
        {
            this.onAttackLeft();
        }
    }

    public void OnAttackFront()
    {
        if(this.onAttackFront != null)
        {
            this.onAttackFront();
        }
    }

    public void OnSummon()
    {
        if(this.onSummon != null)
        {
            this.onSummon();
        }
    }
}
