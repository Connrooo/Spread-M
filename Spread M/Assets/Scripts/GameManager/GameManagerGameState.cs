using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
        //AudioManager.Instance.PlayMusic("Game Music");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx._InGame = false;
        //AudioManager.Instance.StopMusic();
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
}
