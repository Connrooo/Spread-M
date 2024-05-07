using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInfectedRunState : HumanBaseState
{
    public HumanInfectedRunState(HumanStateMachine currentContext, HumanStateFactory humanStateFactory)
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
        CheckIfTargetExists();
        CheckSwitchStates();
        if (Ctx._IsChasing)
        {
            CheckHumanDistance();
        }
    }
    public override void ExitState() 
    {
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx._IsChasing)
        {
            SwitchState(Factory.Walk());
        }
        else if (Ctx.InfectedOnCooldown)
        {
            SwitchState(Factory.Idle());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void CheckIfTargetExists()
    {
        if (Ctx._TrackedHuman==null)
        {
            if (Ctx._FoundHuman != null)
            {
                Ctx._TrackedHuman = Ctx._FoundHuman;
                Ctx._FoundHuman = null;
            }
            else
            {
                Ctx._IsChasing = false;
            }
        }
    }

    private void CheckHumanDistance()
    {
        if (Vector3.Distance(Ctx.transform.position,Ctx._TrackedHuman.transform.position) >= 10)
        {
            Ctx._IsChasing = false;
        }
        else
        {
            ChaseHuman();
        }
    }

    private void ChaseHuman()
    {
        if (Vector3.Distance(Ctx.transform.position, Ctx._TrackedHuman.transform.position) <= 1.1)
        {
            Ctx.InfectedOnCooldown = true;
            var humanScript = Ctx._TrackedHuman.GetComponent<HumanStateMachine>();
            humanScript._IsHuman = false;
            humanScript.HumanAnimator.SetTrigger("infected");
            Ctx._TrackedHuman = null;
            Ctx.HumanAnimator.SetTrigger("cooldown");
        }
        else
        {
            Ctx._HumanAgent.SetDestination(Ctx._TrackedHuman.transform.position);
        }
    }

}
