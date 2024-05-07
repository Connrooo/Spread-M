using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour
{
    [SerializeField]HumanStateMachine parent;
    private void Awake()
    {
        parent = GetComponentInParent<HumanStateMachine>();
        Physics.IgnoreCollision(GetComponent<MeshCollider>(), parent.GetComponent<CapsuleCollider>());
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Interact"))
        {
            if (parent._IsHuman)
            {
                if (col.gameObject.GetComponent<HumanStateMachine>()._IsInfected)
                {
                    parent._FoundHuman = col.gameObject;
                }
            }
            if (parent._IsInfected)
            {
                if (col.gameObject.GetComponent<HumanStateMachine>()._IsHuman)
                {
                    parent._FoundHuman = col.gameObject;
                }
            }
        }
        if (col.CompareTag("Player"))
        {
            if (parent._IsHuman)
            {
                parent._FoundHuman = col.gameObject;
            } 
        }
    }
}
