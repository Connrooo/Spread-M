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
            Ctx.HumanAnimators[0].ResetTrigger("Attacking");
            Ctx.HumanAnimators[1].ResetTrigger("Attacking");
            SwitchState(Factory.Walk());
        }
        else
        {
            ChangeSkins();
            if (!Ctx._IsInfected&&!Ctx._IsCaught)
            {
                Ctx._HumanAgent.isStopped = false;
                Ctx._HumanAgent.speed = 2f * Ctx._HaltedSpeed;
                SwitchState(Factory.Walk());
            }
            Ctx.HumanAnimators[0].SetTrigger("Attacking");
            Ctx.HumanAnimators[1].SetTrigger("Attacking");
        }
    }
    public override void InitializeSubState() 
    {
    }

    private void ChangeSkins()
    {
        if (!Ctx._IsHuman)
        {
            float AnimProgress = Ctx.HumanAnimators[0].GetCurrentAnimatorStateInfo(0).normalizedTime;
            float ExtraProgress = 1.2f;
            if (Ctx.HumanAnimators[0].GetCurrentAnimatorStateInfo(0).IsName("HumanAttacked - H"))
            {
                Ctx.ClonedMaterials[0].SetFloat("_DissolveValue", AnimProgress * ExtraProgress);
                Ctx.ClonedMaterials[1].SetFloat("_DissolveValue", 1 - (AnimProgress * ExtraProgress * 1.7f));
                Ctx.ClonedMaterials[2].SetFloat("_DissolveValue", 1 - (AnimProgress * ExtraProgress * 1.7f));
            }
        }
    }

}
