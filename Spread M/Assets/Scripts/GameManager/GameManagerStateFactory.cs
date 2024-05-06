using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStateFactory
{
    GameManagerStateMachine context;

    public GameManagerStateFactory(GameManagerStateMachine currentContext)
    {
        context = currentContext;
    }

    public GameManagerBaseState Menu()
    {
        return new GameManagerMenuState(context, this);
    }
    public GameManagerBaseState Game()
    {
        return new GameManagerGameState(context, this);
    }
}
