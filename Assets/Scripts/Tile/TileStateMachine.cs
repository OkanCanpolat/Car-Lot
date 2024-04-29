using Zenject;

public class TileStateMachine
{
    public ITileState CurrentState;
    public TileStateMachine([Inject(Id = "T_Idle")] ITileState initialState)
    {
        CurrentState = initialState;
    }
    public void ChangeState(ITileState state)
    {
        CurrentState = state;
    }
}
