using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCone : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    public Material highlighted;
    public Material nothighlighted;
    private void Start()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Interact"))
        {
            playerStateMachine._InteractablesInCone.Add(col.gameObject);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Interact"))
        {
            playerStateMachine._InteractablesInCone.Remove(col.gameObject);
        }
    }
    private void Update()
    {
        if (playerStateMachine._InteractablesInCone.Count > 0)
        {
            GetComponent<Renderer>().material = highlighted;
        }
        else
        {
            GetComponent<Renderer>().material = nothighlighted;
        }
    }
}
