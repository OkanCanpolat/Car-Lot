using Zenject;

public class StickmanStateMachine
{
    public IStickmanState CurrentState;
    public StickmanStateMachine([Inject(Id = "Stc_Idle")] IStickmanState initialState)
    {
        CurrentState = initialState;
        ChangeState(CurrentState);
    }
    public void ChangeState(IStickmanState state)
    {
        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();
    }
}
