using Zenject;

public class TileIdleState : ITileState
{
    private GridSystem gridSystem;
    [Inject(Id = "T_Alert")] private TileAlertState alerState;
    public TileIdleState(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }
    public void OnClickTile(Tile source)
    {
        if (source.Stickman != null)
        {
            alerState.SelectedStickman = source.Stickman;
            source.Stickman.StateMachine.ChangeState(source.Stickman.SelectedState);
            gridSystem.StateMachine.ChangeState(alerState);
        }
    }
    public void OnClickedCar(Car car)
    {
    }
}
