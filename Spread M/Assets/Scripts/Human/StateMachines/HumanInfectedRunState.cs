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
        Ctx._HumanAgent.isStopped = false;
        Ctx._HumanAgent.ResetPath();
        Ctx.HumanAnimators[0].SetBool("Running", true);
        Ctx.HumanAnimators[1].SetBool("Running", true);
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
            Ctx._TrackedHuman = Ctx._FoundHuman;
            Ctx._FoundHuman = null;
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
            humanScript.HumanAnimators[0].SetTrigger("Attacked");
            humanScript.HumanAnimators[1].SetTrigger("Attacked");
            Ctx._TrackedHuman = null;
            Ctx.HumanAnimators[0].SetTrigger("Attacking");
            Ctx.HumanAnimators[1].SetTrigger("Attacking");
        }
        else
        {
            Ctx._HumanAgent.SetDestination(Ctx._TrackedHuman.transform.position);
        }
    }

}
