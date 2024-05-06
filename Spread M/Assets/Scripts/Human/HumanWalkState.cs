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
    public override void EnterState() { }
    public override void UpdateState() 
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
    }
    public override void InitializeSubState() 
    {
    }
}
