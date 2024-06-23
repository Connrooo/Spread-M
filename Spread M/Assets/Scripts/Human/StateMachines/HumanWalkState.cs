using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWalkState : HumanBaseState
{
    public HumanWalkState(HumanStateMachine currentContext, HumanStateFactory humanStateFactory)
    : base(currentContext, humanStateFactory) 
    {
        IsRootState= true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        Ctx._HumanAgent.isStopped = false;
        Ctx.HumanAnimators[0].SetBool("Running", false);
        Ctx.HumanAnimators[1].SetBool("Running", false);
        Ctx._HumanAgent.speed = 3f * Ctx._HaltedSpeed;
    }
    public override void UpdateState() 
    {
        CheckSwitchStates();
        LookForHuman();
        IdleWalk();
        Ctx.CheckIfWalking();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        if (!Ctx._IsHuman&&!Ctx._IsInfected)
        {
            SwitchState(Factory.Idle());
        }
        else
        {
            if (Ctx._IsAfraid)
            {
                SwitchState(Factory.Run());
            }
            else if (Ctx._IsChasing)
            {
                SwitchState(Factory.InfectedRun());
            }
        }
    }
    public override void InitializeSubState() 
    {
    }

    private void LookForHuman()
    {
        if (Ctx._FoundHuman!= null)
        {
            var mask = Ctx._WallMask;
            if (!Physics.Linecast(Ctx.gameObject.transform.position, Ctx._FoundHuman.transform.position, mask))
            {
                if (Ctx._IsHuman)
                {
                    Ctx._IsAfraid = true;
                }
                else if (Ctx._IsInfected)
                {
                    Ctx._IsChasing = true;
                }
                Ctx._TrackedHuman = Ctx._FoundHuman;
            }
        }
    }
    private void IdleWalk()
    {
        if (!Ctx._IsWalking)
        {
            bool repeat = false;
            Ctx.NewDestination(repeat, Ctx._WalkingRange);
            if (repeat)
            {
                IdleWalk();
            }
        }
        else
        {
            if (Ctx.pathComplete())
            {
                Ctx._IsWalking = false;
            }
        }
    }
    
    
}
