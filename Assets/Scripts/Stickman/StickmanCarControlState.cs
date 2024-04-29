using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickmanCarControlState : IStickmanState
{
    private Car selectedCar;
    private Stickman stickman;
    private IPathFinder pathFinder;
    private IEmotion emotion;
    private IIndicator carIndicator;
    [Inject(Id = "T_Alert")] private TileAlertState tileAlert;
    [Inject(Id = "Stc_Idle")] private StickmanIdleState idleState;
    [Inject(Id = "Stc_Movement")] private StickmanMovementState movementState;
    [Inject(Id = "Stc_EnterCar")] private StickmanCarEnterState carEnter;

    public StickmanCarControlState(Stickman stickman, IPathFinder pathFinder)
    {
        this.stickman = stickman;
        this.pathFinder = pathFinder;
        emotion = stickman.GetComponent<IEmotion>();
    }
    public void OnEnter()
    {
        selectedCar = tileAlert.SelectedCar;
        carIndicator = selectedCar.GetComponent<IIndicator>();

        if (selectedCar.Color != stickman.Color)
        {
            emotion.Angry();
            carIndicator.ShowIndicator(Color.red);
            stickman.StateMachine.ChangeState(idleState);
        }

        else
        {
            TryReachCar();
        }
    }
    public void OnExit()
    {
    }
    private List<Tile> GetClosestPath(List<List<Tile>> paths)
    {
        float distance = float.MaxValue;
        List<Tile> closestPath = paths[0];

        foreach (List<Tile> path in paths)
        {

            float currentDistance = Vector3.Distance(stickman.CurrentTile.transform.position, path[path.Count - 1].transform.position);
            
            if(currentDistance < distance)
            {
                closestPath = path;
                distance = currentDistance;
            }
        }
        return closestPath;
    }
    private List<List<Tile>> GetValidPaths()
    {
        List<List<Tile>> validPaths = new List<List<Tile>>();

        foreach (Tile seatTile in selectedCar.SeatTiles)
        {
            List<Tile> path = pathFinder.FindPath(stickman.CurrentTile.X, stickman.CurrentTile.Y, seatTile.X, seatTile.Y);

            if (path != null) validPaths.Add(path);
        }
        return validPaths;
    }
    private void TryReachCar()
    {
        List<List<Tile>> validPaths = GetValidPaths();
        List<Tile> closestPath;

        if (validPaths.Count > 0)
        {
            carIndicator.ShowIndicator(Color.green);
            closestPath = GetClosestPath(validPaths);
            emotion.Happy();
            stickman.TargetTile = closestPath[closestPath.Count - 1];
            movementState.TargetPath = closestPath;
            stickman.CurrentTile.Stickman = null;
            stickman.CurrentTile.IsBlocked = false;
            stickman.StateMachine.ChangeState(stickman.MovementState);
            stickman.StartCoroutine(GetInCar());
            return;
        }

        else
        {
            IIndicator tileIndicator = selectedCar.SeatTiles[0].GetComponent<IIndicator>();
            tileIndicator.ShowIndicator(Color.red);
            carIndicator.ShowIndicator(Color.red);
            emotion.Angry();
            stickman.StateMachine.ChangeState(idleState);
        }
    }
    private IEnumerator GetInCar()
    {
        while (movementState.IsMoving)
        {
            yield return null;
        }
        carEnter.Car = selectedCar;
        stickman.StateMachine.ChangeState(stickman.CarEnterState);
    }
}
