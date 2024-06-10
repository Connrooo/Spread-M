using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        IsRootState= true;
        InitializeSubState();
    }
    public override void EnterState() { }
    public override void UpdateState() 
    {
        Ctx.CharacterAnimator.SetBool("Walking", false);
        CheckSwitchStates();
        FieldOfViewAdjust();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        if (Ctx.gameManager._CanMove)
        {
            if (Ctx.vertInput != 0 || Ctx.horInput != 0)
            {
                SwitchState(Factory.Walk());
            }
        }
        SetSubState(Factory.Interact());
    }
    public override void InitializeSubState() 
    {
        SetSubState(Factory.Interact());
    }

    private void FieldOfViewAdjust()
    {
        if (Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView > Ctx._WalkingFOV)
        {
            Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView = Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView - 25 * Time.deltaTime;
        }
        else
        {
            Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView = Ctx._WalkingFOV;
        }
    }
}
