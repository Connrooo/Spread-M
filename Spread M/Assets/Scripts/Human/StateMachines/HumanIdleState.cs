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
        if (Ctx._IsInfected&&!Ctx.InfectedOnCooldown)
        {
            Ctx._HumanAgent.speed = 3f * Ctx._HaltedSpeed;
            Ctx._TrackedHuman = null;
            Ctx._FoundHuman = null;
            Ctx._IsAfraid = false;
            SwitchState(Factory.Walk());
        }
        else
        {
            if (!Ctx._IsCaught)
            {
                Ctx._HumanAgent.speed = 5f * Ctx._HaltedSpeed;
                SwitchState(Factory.Walk());
            }
        }
        Ctx._HumanAgent.SetDestination(Ctx.transform.position);
    }
    public override void InitializeSubState() 
    {
    }
}
