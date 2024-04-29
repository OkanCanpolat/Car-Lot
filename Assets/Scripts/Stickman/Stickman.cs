using UnityEngine;
using Zenject;

public enum MatchColor
{
    Red, Blue, Green, Yellow, Black, Orange, Purple, Pink, None
}
public class Stickman : MonoBehaviour
{
    public StickmanStateMachine StateMachine;
    [SerializeField] private MatchColor color;
    private Tile currentTile;
    private Tile targetTile;
    private GridSystem gridSystem;
    #region States
    [Inject(Id = "Stc_Movement")] public IStickmanState MovementState;
    [Inject(Id = "Stc_Idle")] public IStickmanState IdleState;
    [Inject(Id = "Stc_Selected")] public IStickmanState SelectedState;
    [Inject(Id = "Stc_CarControl")] public IStickmanState CarState;
    [Inject(Id = "Stc_PathFinding")] public IStickmanState PathFindingState;
    [Inject(Id = "Stc_EnterCar")] public IStickmanState CarEnterState;
    #endregion

    #region Getter/Setter
    public MatchColor Color => color;
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }
    public Tile TargetTile { get => targetTile; set => targetTile = value; }
    #endregion

    [Inject]
    public void Construct(StickmanStateMachine stateMachine, GridSystem gridSystem)
    {
        StateMachine = stateMachine;
        this.gridSystem = gridSystem;
    }

    private void Start()
    {
        currentTile = gridSystem.GetTile(transform.position);
        transform.position = currentTile.transform.position;
        currentTile.Stickman = this;
        currentTile.IsBlocked = true;
    }
}
