using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractState : PlayerBaseState
{
    public PlayerInteractState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    GameObject objectHighlighted;

    public override void EnterState() { }
    public override void UpdateState() 
    {
        if (Ctx.gameManager._CanMove && !Ctx.gameManager._Paused)
        {
            InteractRaycast();
        }
    }
    public override void ExitState() 
    {
    }
    public override void CheckSwitchStates() 
    {
    }
    public override void InitializeSubState() { }

    private void InteractRaycast()
    {
        List<GameObject> toDelete = new List<GameObject>();
        foreach(GameObject interactable in Ctx._InteractablesInCone)
        {
            var mask = Ctx.layerMaskInteract.value;
            if (!Physics.Linecast(Ctx._PlayerTrack.position, interactable.transform.position, mask) && Ctx.IsInteractPressed)
            {
                toDelete.Add(interactable);
            }
        }
        foreach(GameObject interactable in toDelete)
        {
            Ctx._InteractablesInCone.Remove(interactable);
            Object.Destroy(interactable);
        }
    }
}
