using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFromAnimations : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }

    public void StartedAttacking()
    {
        playerStateMachine.CharacterAnimator.ResetTrigger("Attack");
        playerStateMachine.cooldownSpeed = .01f;
    }
    public void ResetCooldownSpeed()
    {
        playerStateMachine.cooldownSpeed = 1f;
    }
}
