using Zenject;

public class TileAlertState : ITileState
{
    public Car SelectedCar;
    public Stickman SelectedStickman;
    private GridSystem gridSystem;
    [Inject(Id = "T_Idle")] private ITileState idleState;

    public TileAlertState(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }
    public void OnClickTile(Tile source)
    {
        if (source.Stickman != null && source.Stickman == SelectedStickman)
        {
            SelectedStickman.StateMachine.ChangeState(source.Stickman.IdleState);
            gridSystem.StateMachine.ChangeState(idleState);
            SelectedStickman = null;
        }
        else if (source.Stickman != null && source.Stickman != SelectedStickman)
        {
            SelectedStickman.StateMachine.ChangeState(SelectedStickman.IdleState);
            SelectedStickman = source.Stickman;
            SelectedStickman.StateMachine.ChangeState(SelectedStickman.SelectedState);
        }
        else
        {
            SelectedStickman.TargetTile = source;
            SelectedStickman.StateMachine.ChangeState(SelectedStickman.PathFindingState);
            gridSystem.StateMachine.ChangeState(idleState);
            SelectedStickman = null;
        }
    }
    public void OnClickedCar(Car car)
    {
        SelectedCar = car;
        SelectedStickman.StateMachine.ChangeState(SelectedStickman.CarState);
        SelectedStickman = null;
        gridSystem.StateMachine.ChangeState(idleState);
    }
}
