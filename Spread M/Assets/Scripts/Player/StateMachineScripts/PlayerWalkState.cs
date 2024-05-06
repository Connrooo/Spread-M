using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    float moveSoundTimer = 0.5f;

    public override void EnterState() 
    {
    }
    public override void UpdateState() 
    {
        CheckSwitchStates();
        LoadMovement();
        
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        if(Ctx.characterController.isGrounded)
        {
            if (Ctx.horInput == 0 && Ctx.vertInput == 0)
            {
                SwitchState(Factory.Idle());
            }
            else if (!Ctx.gameManager._CanMove)
            {
                SwitchState(Factory.Idle());
            }
        }
        SetSubState(Factory.Interact());
    }
    public override void InitializeSubState() 
    {
        SetSubState(Factory.Interact());
    }

    private void CheckForSprint()
    {
        if (Ctx.IsSprintPressed)
        {
            if (Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView < Ctx._SprintingFOV)
            {
                Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView = Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView + 30 * Time.deltaTime;
            }
            else
            {
                Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView = Ctx._SprintingFOV;
            }
            Ctx._Sprinting = Ctx.SprintMult;
        }
        else
        {
            if (Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView > Ctx._WalkingFOV)
            {
                Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView = Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView - 25 * Time.deltaTime;
            }
            else
            {
                Ctx.gameManager.gameplayCamera.m_Lens.FieldOfView = Ctx._WalkingFOV;
            }
            Ctx._Sprinting = 1;
        }
    }

    private void ApplyPlayerInput()
    {
        Ctx.moveDirection = Ctx.cameraObject.forward * Ctx.vertInput;
        Ctx.moveDirection = Ctx.moveDirection + Ctx.cameraObject.right * Ctx.horInput;
        Ctx.moveDirection.y = 0f;
    }

    private void CheckGravity()
    {
        if (!Ctx.characterController.isGrounded)
        {
            Ctx.moveDirection.y -= Ctx._Gravity * Time.deltaTime;
            if (Ctx.characterController.collisionFlags == CollisionFlags.Above)
            {
                Ctx.characterController.stepOffset = 0;
            }
        }
        else
        {
            if (Ctx.characterController.stepOffset == 0)
            {
                Ctx.characterController.stepOffset = 0.3f;
            }
        }
    }

    private void ApplyMovement()
    {
        Ctx.characterController.Move(Ctx.moveDirection * Ctx.movementSpeed * Ctx._Sprinting * Time.deltaTime);
    }

    private void ApplySound()
    {
        if (Ctx.moveDirection.x * Ctx.movementSpeed * Time.deltaTime != 0 || Ctx.moveDirection.z * Ctx.movementSpeed * Time.deltaTime != 0)
        {
            if (moveSoundTimer <= 0)
            {
                moveSoundTimer = 0.5f;
                //AudioManager.Instance.PlayWalk();
            }
            else
            {
                moveSoundTimer -= Time.deltaTime;
            }
        }
    }
    private void LoadMovement()
    {
        CheckForSprint();
        ApplyPlayerInput();
        CheckGravity();
        ApplyMovement();
        ApplySound();
    }
}
