using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManagerStateMachine : MonoBehaviour
{
    GameManagerBaseState currentState;
    GameManagerStateFactory states;
    public GameManagerBaseState CurrentState { get { return currentState; } set { currentState = value; } }

    [Header("Global Values")]
    public int amountOfHumans;
    public GameObject[] humans;
    public GameObject Player;
    public MenuManager menuManager;
    public SettingsMenu settingsMenu;
    public InputManager inputManager;
    public AudioManager audioManager;
    public bool _Paused; //checks if the game is paused
    public bool _PlayingGame; //checks if the game is being played or not (will be in menu if not being played)
    public bool _CanMove;
    public bool _InGame;
    public bool _InMenu;

    public GameObject HumanPrefab;

    public GameObject _DeletableObjectPrefab;
    public GameObject _DeletableObjects;
    public Vector3[] _possibleCoordinates;
    public Vector3 PlayerStartPosition;
    public Quaternion PlayerStartRotation;
    public Quaternion CameraStartRotation;
    public bool ResetRotation;

    [Header("Cinemachine")]
    public List<CinemachineVirtualCamera> _Cameras;
    public CinemachineVirtualCamera menuCamera;
    public CinemachineVirtualCamera gameplayCamera;
    public GameObject CameraTrack;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(menuManager);
        DontDestroyOnLoad(settingsMenu);
        DontDestroyOnLoad(inputManager);
        DontDestroyOnLoad(audioManager);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        states = new GameManagerStateFactory(this);
        _Paused = true;
    }

    private void Start()
    {
        CameraTrack = GameObject.FindGameObjectWithTag("CameraTrack");
        gameplayCamera = GameObject.FindGameObjectWithTag("GameplayCamera").GetComponent<CinemachineVirtualCamera>();
        menuCamera = GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<CinemachineVirtualCamera>();
        _Cameras.Add(gameplayCamera);_Cameras.Add(menuCamera);
        Player = GameObject.FindGameObjectWithTag("Player");
        currentState = states.Menu();
        currentState.EnterState();
        
    }
    private void Update()
    {
        currentState.UpdateStates();
        if (_Paused || !_PlayingGame)
        {
            _CanMove = false;
        }
        else
        {
            _CanMove = true;
        }
        if (amountOfHumans==0)
        {
            Debug.Log("Game over");
        }
        Debug.Log(settingsMenu.NumberOfHumans);
    }
}
