using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Car : MonoBehaviour
{
    public CarStateMachine StateMachine;
    [SerializeField] private MatchColor color;
    [Header("Seat Variables")]
    [SerializeField] private Tile leftSeatTile;
    [SerializeField] private Tile rightSeatTile;
    [Header("Get In Variables")]
    [SerializeField] private Transform seatPosition;
    [SerializeField] private Transform leftOpenPosition;
    [SerializeField] private Transform rightOpenPosition;
    [SerializeField] private Transform leftOpenLookAt;
    [SerializeField] private Transform rightOpenLookAt;
    private GridSystem gridSystem;
    private Animator animator;
    private List<Tile> seatTiles;

    #region Getter/Setter
    public List<Tile> SeatTiles => seatTiles;
    public MatchColor Color => color;
    public Transform SeatPosition => seatPosition;
    #endregion

    [Inject] public CarIdleState IdleState;

    [Inject]
    public void Construct(GridSystem gridSystem, CarStateMachine stateMachine)
    {
        this.gridSystem = gridSystem;
        StateMachine = stateMachine;
        animator = GetComponent<Animator>();
        seatTiles = new List<Tile>();
    }

    private void Awake()
    {
        if (rightSeatTile != null) SeatTiles.Add(rightSeatTile);
        if (leftSeatTile != null) SeatTiles.Add(leftSeatTile);
    }
    private void OnMouseDown()
    {
        gridSystem.StateMachine.CurrentState.OnClickedCar(this);
    }
    public Transform GetEnterPosition(Tile tile)
    {
        if (tile == rightSeatTile) return rightOpenPosition;
        return leftOpenPosition;
    }
    public Transform GetEnterLookAt(Tile tile)
    {
        if (tile == rightSeatTile) return rightOpenLookAt;
        return leftOpenLookAt;
    }
    public IEnumerator OpenDoor(Tile tile)
    {
        string animationStateName;

        if (tile == rightSeatTile)
        {
            animator.SetTrigger("Open_R");
            animationStateName = "Open_R";
        }
        else
        {
            animator.SetTrigger("Open_L");
            animationStateName = "Open_L";
        }

        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
        {
            yield return null;
        }
    }
    public IEnumerator CloseDoor(Tile tile)
    {
        string animationStateName;
        if (tile == rightSeatTile)
        {
            animator.SetTrigger("Close_R");
            animationStateName = "Close_R";
        }
        else
        {
            animator.SetTrigger("Close_L");
            animationStateName = "Close_L";
        }
        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
        {
            yield return null;
        }
    }
    public void ActivateTiles()
    {
        TileBlock tileBlock = GetComponent<TileBlock>();

        foreach (Tile tile in tileBlock.BlockedTiles)
        {
            tile.GetComponent<Collider>().enabled = true;
            tile.IsBlocked = false;
        }
    }
}
