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
    public override void EnterState() { }
    public override void UpdateState() 
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        if (Ctx._IsHuman)
        {
            SwitchState(Factory.Walk());
        }
    }
    public override void InitializeSubState() 
    {
    }
}
