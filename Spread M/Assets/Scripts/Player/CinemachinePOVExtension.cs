using UnityEngine;
using Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngleUp = 40f;
    [SerializeField] private float clampAngleDown = 60f;
    [SerializeField] private GameManagerStateMachine gameManager;
    PlayerStateMachine playerStateMachine;
    GameObject CameraTrack;
    Vector3 startingRotation;

    protected override void Awake()
    {
        playerStateMachine = FindAnyObjectByType<PlayerStateMachine>();
        gameManager= FindAnyObjectByType<GameManagerStateMachine>();
        base.Awake();
        startingRotation = transform.localRotation.eulerAngles;
    }

    private void Start()
    {
        CameraTrack = gameManager.CameraTrack;
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow&&gameManager._PlayingGame)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = new Vector2(playerStateMachine.cameraInputX, playerStateMachine.cameraInputY);
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngleDown, clampAngleUp);
                if (gameManager.ResetRotation)
                {
                    gameManager.ResetRotation = false;
                    deltaInput = new Vector2();
                    startingRotation = new Vector3(0, 0, 0);
                    //startingRotation = gameManager.CameraStartRotation.eulerAngles;
                    //startingRotation.y = -startingRotation.y;
                }
                CameraTrack.transform.rotation = Quaternion.Euler(-startingRotation.y, startingRotation.x, CameraTrack.transform.rotation.eulerAngles.z);
            }
        }
    }
}

