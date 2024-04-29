using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickmanPathFindingState : IStickmanState
{
    private Stickman stickman;
    private IPathFinder pathFinder;
    private IEmotion emotion;
    [Inject(Id = "Stc_Idle")] private StickmanIdleState idleState;
    [Inject(Id = "Stc_Movement")] private StickmanMovementState movementState;

    public StickmanPathFindingState(Stickman stickman,  IPathFinder pathFinder)
    {
        this.stickman = stickman;
        this.pathFinder = pathFinder;
        emotion = stickman.GetComponent<IEmotion>();
    }
    public void OnEnter()
    {
        List<Tile> path = pathFinder.FindPath(stickman.CurrentTile.X, stickman.CurrentTile.Y, stickman.TargetTile.X, stickman.TargetTile.Y);
        IIndicator indicator = stickman.TargetTile.GetComponent<IIndicator>();

        if (path == null)
        {
            indicator.ShowIndicator(Color.red);
            stickman.StateMachine.ChangeState(idleState);
            emotion.Angry();
        }
        else
        {
            indicator.ShowIndicator(Color.green);
            stickman.CurrentTile.Stickman = null;
            stickman.CurrentTile.IsBlocked = false;
            stickman.CurrentTile = stickman.TargetTile;
            stickman.CurrentTile.IsBlocked = true;
            stickman.TargetTile.Stickman = stickman;
            movementState.TargetPath = path;
            stickman.StateMachine.ChangeState(stickman.MovementState);
        }
    }
    public void OnExit()
    {
    }
}
