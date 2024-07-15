using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class GameManagerGameState : GameManagerBaseState
{
    public GameManagerGameState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
    : base(currentContext, gameManagerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        foreach(CinemachineVirtualCamera camera in Ctx._Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.gameplayCamera.Priority = 11;
        Ctx._DeletableObjects = Object.Instantiate(Ctx._DeletableObjectPrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        SetPlayer();
        SummonHumans(5);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx._InGame = false;
        ResetGame();
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx._PlayingGame)
        {
            SwitchState(Factory.Menu());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void SetPlayer()
    {
        Ctx.PlayerStartPosition = Ctx.Player.transform.position;
        Ctx.PlayerStartRotation = Ctx.Player.transform.rotation;
        Ctx.CameraStartRotation = Ctx.gameplayCamera.transform.rotation;
    }

    private void SummonHumans(int desiredNumberOfHumans)
    {
        List<Vector3> humanCoordinates = new();
        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("PossibleCoordinates");
        List<Vector3> possibleCoordinates = new();
        foreach (GameObject spawnPosition in spawnPositions) 
        {
            possibleCoordinates.Add(spawnPosition.transform.position);
        }
        for (int i = 0; i < desiredNumberOfHumans; i++)
        {
            CheckForPosition(possibleCoordinates, humanCoordinates);
        }
        foreach(Vector3 humanCoordinate in humanCoordinates)
        {
            GameObject human = Object.Instantiate(Ctx.HumanPrefab, humanCoordinate, Ctx.HumanPrefab.transform.rotation);
            human.transform.parent = Ctx._DeletableObjects.transform;
        }
        CheckAmountOfHumans();
    }

    private void CheckForPosition(List<Vector3> possibleCoordinates, List<Vector3> humanCoordinates)
    {
        Vector3 newPosition = possibleCoordinates[Random.Range(0, possibleCoordinates.Count)];
        if (humanCoordinates.Contains(newPosition))
        {
            CheckForPosition(possibleCoordinates, humanCoordinates);
        }
        else
        {
            humanCoordinates.Add(newPosition);
        }
    }

    private void CheckAmountOfHumans()
    {
        Ctx.humans = GameObject.FindGameObjectsWithTag("Interact");
        foreach (GameObject human in Ctx.humans)
        {
            Ctx.amountOfHumans++;
        }
    }

    private void ResetGame()
    {
        Object.Destroy(Ctx._DeletableObjects);
        Ctx.amountOfHumans = 0;
        Ctx.Player.transform.position = Ctx.PlayerStartPosition;
        Ctx.Player.transform.rotation = Ctx.PlayerStartRotation;
        Ctx.ResetRotation = true;
    }

}
