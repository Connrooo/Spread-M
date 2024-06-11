using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanIdleState : HumanBaseState
{
    public HumanIdleState(HumanStateMachine currentContext, HumanStateFactory humanStateFactory)
    : base(currentContext, humanStateFactory) 
    {
        IsRootState= true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        Ctx._HumanAgent.ResetPath();
    }
    public override void UpdateState() 
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        Ctx._HumanAgent.isStopped = true;
        if (Ctx._IsInfected&&!Ctx.InfectedOnCooldown)
        {
            Ctx._HumanAgent.isStopped = false;
            Ctx._HumanAgent.speed = 3f * Ctx._HaltedSpeed;
            Ctx._TrackedHuman = null;
            Ctx._FoundHuman = null;
            Ctx._IsAfraid = false;
            Ctx.HumanAnimator.ResetTrigger("Attacking");
            SwitchState(Factory.Walk());
        }
        else
        {
            if (!Ctx._IsInfected&&!Ctx._IsCaught)
            {
                Ctx._HumanAgent.isStopped = false;
                Ctx._HumanAgent.speed = 2f * Ctx._HaltedSpeed;
                SwitchState(Factory.Walk());
            }
            Ctx.HumanAnimator.SetTrigger("Attacking");
        }
    }
    public override void InitializeSubState() 
    {
    }
}
