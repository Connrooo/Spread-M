using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStateFactory
{
    HumanStateMachine context;

    public HumanStateFactory(HumanStateMachine currentContext)
    {
        context = currentContext;
    }

    
    public HumanBaseState Walk()
    {
        return new HumanWalkState(context, this);
    }
    public HumanBaseState Run()
    {
        return new HumanRunState(context, this);
    }
    public HumanBaseState Idle()
    {
        return new HumanIdleState(context, this);
    }
    public HumanBaseState InfectedRun()
    {
        return new HumanInfectedRunState(context, this);
    }
}
