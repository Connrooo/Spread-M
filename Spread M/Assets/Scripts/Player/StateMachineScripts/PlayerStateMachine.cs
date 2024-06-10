using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerStateMachine : MonoBehaviour
{
    public Transform cameraObject;
    public Transform _PlayerTrack;
    public GameManagerStateMachine gameManager;
    public MenuManager menuManager;
    public SettingsMenu settingsMenu;
    public RebindUI rebindUI;
    public CharacterController characterController;

    public Animator CharacterAnimator;
    public GameObject CharacterRotation;

    [Header("Player Walk Variables")]
    public Vector3 moveDirection;
    public float movementSpeed = 10;
    public float cooldownSpeed = 1;
    public float _Gravity = 400f;

    [Header("Sprint Variables")]
    public float SprintMult = 2f;
    public float _Sprinting = 1;
    public float _WalkingFOV = 40f;
    public float _SprintingFOV = 60f;

    [Header("Player Interact Variables")]
    //public TMP_Text InteractPromptText;
    //public Animator InteractPromptTextAnim;
    public float rayLength = 2;
    public LayerMask layerMaskInteract;
    public List<GameObject> _InteractablesInCone;

    [Header("Input Start Up")]
    public PlayerInputSystem PlayerInput;

    [Header("Movement Controls")]
    Vector2 movementInput;
    public InputActionReference walkActionReference;
    [Header("Camera Controls")]
    Vector2 cameraInput;
    [Header("Camera Values")]
    public float cameraInputX;
    public float cameraInputY;
    public float _SensitivityMultiplier;
    [Header("Interact Button")]
    public bool IsInteractPressed;
    [Header("Sprint Button")]
    public bool IsSprintPressed;
    [Header("Menu Open Close Button")]
    public bool IsMenuOpenClosePressed;

    [Header("Vert/Hor Input")]
    public float vertInput;
    public float horInput;

    PlayerBaseState currentState;
    PlayerStateFactory states;

    public PlayerBaseState CurrentState { get { return currentState;} set { currentState = value; } }

    private void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>();
        settingsMenu = FindObjectOfType<SettingsMenu>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        characterController = GetComponent<CharacterController>();
        //menuManager.PlayerUI.SetActive(true);
        //InteractPromptText = GameObject.FindWithTag("InteractPromptText").GetComponent<TMP_Text>();
        //InteractPromptTextAnim = GameObject.FindWithTag("InteractPromptText").GetComponent<Animator>();
        //menuManager.PlayerUI.SetActive(false);
        cameraObject = Camera.main.transform;
        states = new PlayerStateFactory(this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        currentState.UpdateStates();
        IsInteractPressed = false;
    }

    private void FixedUpdate()
    {
        MoveInputHandler();
    }

    private void OnEnable()
    {
        PlayerInput = InputManager.PlayerInput;
        PlayerInput.Enable();
        PlayerInput.Main.Movement.performed += OnMove;
        PlayerInput.Main.Movement.canceled += OnMove;
        PlayerInput.Main.Turn.performed += OnTurn;
        PlayerInput.Main.Turn.canceled += OnTurn;
        PlayerInput.Main.Interact.started += OnInteract;
        PlayerInput.Main.Sprint.performed += OnSprint;
        PlayerInput.Main.Sprint.canceled += OnSprint;
        PlayerInput.Main.MenuOpenClose.started += OnMenuOpenClose;
    }

    private void OnDisable()
    {
        PlayerInput = InputManager.PlayerInput;
        PlayerInput.Disable();
        PlayerInput.Main.Movement.performed -= OnMove;
        PlayerInput.Main.Movement.canceled -= OnMove;
        PlayerInput.Main.Turn.performed -= OnTurn;
        PlayerInput.Main.Turn.canceled -= OnTurn;
        PlayerInput.Main.Interact.started -= OnInteract;
        PlayerInput.Main.Sprint.performed -= OnSprint;
        PlayerInput.Main.Sprint.canceled -= OnSprint;
        PlayerInput.Main.MenuOpenClose.started += OnMenuOpenClose;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    private void OnTurn(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        IsInteractPressed = ctx.ReadValueAsButton();
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        IsSprintPressed = ctx.ReadValueAsButton();
    }

    private void OnMenuOpenClose(InputAction.CallbackContext ctx)
    {
        IsMenuOpenClosePressed = ctx.ReadValueAsButton();
    }

    private void MoveInputHandler()
    {
        vertInput = movementInput.y;
        horInput = movementInput.x;
        cameraInputY = cameraInput.y * settingsMenu._SensitivityMultiplier;
        cameraInputX = cameraInput.x * settingsMenu._SensitivityMultiplier;
    }
}
