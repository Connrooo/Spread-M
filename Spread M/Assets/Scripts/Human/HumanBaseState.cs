public abstract class HumanBaseState
{
    private bool isRootState = false;
    private HumanStateMachine ctx;
    private HumanStateFactory factory;
    private HumanBaseState currentSuperState;
    private HumanBaseState currentSubState;

    protected bool IsRootState { set { isRootState = value; } }
    protected HumanStateMachine Ctx { get { return ctx; } }
    protected HumanStateFactory Factory { get { return factory; } }

    public HumanBaseState(HumanStateMachine currentContext, HumanStateFactory humanStateFactory)
    {
        ctx = currentContext;
        factory = humanStateFactory;
    }


    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }
    protected void SwitchState(HumanBaseState newState) 
    {
        ExitState(); // Current state exits state
        newState.EnterState(); // New state enters state
        if (isRootState)
        {
            ctx.CurrentState = newState; // Switches current state of context
        }
        else if (currentSuperState != null)
        {
            currentSuperState.SetSubState(newState); // Sets the current super states sub state to the new state
        }
    }
    protected void SetSuperState(HumanBaseState newSuperState) 
    {
        currentSuperState = newSuperState;
    }
    protected void SetSubState(HumanBaseState newSubState) 
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
