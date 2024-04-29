using UnityEngine;
using Zenject;

public class Tile : MonoBehaviour
{
    private TileStateMachine stateMachine;
    private int x;
    private int y;
    private bool isBlocked;
    private Stickman stickman;

    private int gCost;
    private int hCost;
    private int fCost;
    private Tile parent;

    #region Getter/Setter
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public int FCost { get => fCost; set => fCost = value; }
    public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
    public Stickman Stickman { get => stickman; set => stickman = value; }
    public Tile Parent { get => parent; set => parent = value; }
    #endregion

    [Inject]
    public void Construct(TileStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    private void OnMouseDown()
    {
        stateMachine.CurrentState.OnClickTile(this);
    }
}
