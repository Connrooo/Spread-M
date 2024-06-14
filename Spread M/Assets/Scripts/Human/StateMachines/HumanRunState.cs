using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRunState : HumanBaseState
{
    public HumanRunState(HumanStateMachine currentContext, HumanStateFactory humanStateFactory)
    : base(currentContext, humanStateFactory) 
    {
        IsRootState= true;
        InitializeSubState();
    }

    float expandingRange;
    float ScaredTime = 30;

    public override void EnterState() 
    {
        Ctx._HumanAgent.isStopped = false;
        Ctx._HumanAgent.ResetPath();
        Ctx._IsWalking = false;
        expandingRange = 1f;
        Ctx._HumanAgent.speed = 10f * Ctx._HaltedSpeed;
        Ctx.HumanAnimators[0].SetBool("Running", true);
        Ctx.HumanAnimators[1].SetBool("Running", true);
    }
    public override void UpdateState() 
    {
        CheckSwitchStates();
        RunFromZombie();
        Ctx.CheckIfWalking();
        CheckIfScared();
        CheckForNewTarget();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        if (!Ctx._IsHuman)
        {
            SwitchState(Factory.Idle());
        }
        else if (!Ctx._IsAfraid)
        {
            SwitchState(Factory.Walk());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void RunFromZombie()
    {
        ScaredTime -= Time.deltaTime;
        expandingRange += (Time.deltaTime / 10);
        //Debug.Log(expandingRange);
        if (!Ctx._IsWalking)
        {
            Vector3 dirToInfected = Ctx.transform.position - Ctx._TrackedHuman.transform.position;
            Vector3 newPos = Ctx._TrackedHuman.transform.position + (dirToInfected*expandingRange);
            Vector3 targetPosition = Ctx.RandomNavSphere(newPos, 5);
            if (targetPosition == Vector3.zero)
            {
                bool repeat = false;
                Ctx.NewDestination(repeat, 10);
                if (repeat)
                {
                    RunFromZombie();
                }
            }
            else
            {
                Ctx._HumanAgent.SetDestination(targetPosition);
                Ctx._IsWalking = true;
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
    private void CheckIfScared()
    {
        if (ScaredTime <= 0)
        {
            Ctx._IsAfraid = false;
            Ctx._FoundHuman = null;
        }  
    }

    private void CheckForNewTarget()
    {
        if (Ctx._TrackedHuman!=null && Ctx._FoundHuman!=null)
        {
            if (Vector3.Distance(Ctx.transform.position, Ctx._TrackedHuman.transform.position) > Vector3.Distance(Ctx.transform.position, Ctx._FoundHuman.transform.position))
            {
                Ctx._TrackedHuman = Ctx._FoundHuman;
            }
        }
    }

}
