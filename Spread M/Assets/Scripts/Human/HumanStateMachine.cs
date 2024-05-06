using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class HumanStateMachine : MonoBehaviour
{
    [Header("NavMesh Components")]
    NavMeshAgent _HumanAgent;

    [Header("Global Values")]
    GameManagerStateMachine gameManager;

    [Header("Human State")]
    public bool _IsHuman;
    public bool _IsInfected;
    
    [Header("Alive States")]
    public bool _IsAfraid;
    public bool _IsCaught;

    [Header("Infected States")]
    public bool _IsChasing;

    HumanBaseState currentState;
    HumanStateFactory states;

    public HumanBaseState CurrentState { get { return currentState;} set { currentState = value; } }

    private void Awake()
    {
        states = new HumanStateFactory(this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    private void Start()
    {
        _IsHuman = true;
    }

    private void Update()
    {
        currentState.UpdateStates();
    }
}
