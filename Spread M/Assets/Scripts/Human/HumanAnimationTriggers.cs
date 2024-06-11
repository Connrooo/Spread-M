using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimationTriggers : MonoBehaviour
{
    HumanStateMachine parent;
    Animator HumanAnimator;
    GameManagerStateMachine gameManager;

    private void Awake()
    {
        parent = GetComponentInParent<HumanStateMachine>();
        HumanAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
    }

    public void StopSpeed()
    {
        //parent._HaltedSpeed = 0;
    }

    public void Converted()
    {
        parent._IsInfected = true;
        parent._HaltedSpeed = 1;
        gameManager.amountOfHumans--;
    }
    public void OffCoolDown()
    {
        HumanAnimator.ResetTrigger("Attacking");
        parent._HaltedSpeed = 1;
        parent.InfectedOnCooldown = false;
    }
}
