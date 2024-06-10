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
        Ctx.CharacterAnimator.SetBool("Walking", true);
        CheckSwitchStates();
        LoadMovement();
        
    }
    public override void ExitState() 
    {
        Ctx.CharacterAnimator.SetBool("Walking", false);
    }
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
            Ctx.CharacterAnimator.SetBool("Running",true);
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
            Ctx.CharacterAnimator.SetBool("Running", false);
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
        Ctx.characterController.Move(Ctx.moveDirection * Ctx.movementSpeed * Ctx._Sprinting * Time.deltaTime * Ctx.cooldownSpeed);
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        Vector3 facingDirection = Ctx.moveDirection;
        var CharacterPosition = Ctx.CharacterRotation.transform.position;
        Vector3 LookedAtPosition = new Vector3((CharacterPosition.x + facingDirection.x), CharacterPosition.y, (CharacterPosition.z + facingDirection.z));
        var targetRotation = Quaternion.LookRotation(LookedAtPosition - Ctx.transform.position);
        Ctx.CharacterRotation.transform.rotation = Quaternion.Slerp(Ctx.CharacterRotation.transform.rotation, targetRotation, 4f * Time.deltaTime*Ctx.cooldownSpeed);
        Ctx.CharacterRotation.transform.rotation = Quaternion.Euler(0, Ctx.CharacterRotation.transform.rotation.eulerAngles.y,0);
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
