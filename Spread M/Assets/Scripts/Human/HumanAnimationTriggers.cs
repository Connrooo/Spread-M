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

    public void Converted()
    {
        parent._IsInfected = true;
        gameManager.amountOfHumans--;
    }
    public void OffCoolDown()
    {
        HumanAnimator.ResetTrigger("cooldown");
        parent.InfectedOnCooldown = false;
    }
}
