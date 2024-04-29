public class CarStateMachine 
{
    public ICarState CurrentState;
    public void ChangeState(ICarState state)
    {
        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();
    }
}
