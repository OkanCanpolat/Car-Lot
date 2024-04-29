using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarIdleState : ICarState
{
    private Car car;
    private Animator animator;
    private CarMovementController carMovementController;
    private List<List<Tile>> pathTiles = new List<List<Tile>>();
    private CarMovementState movementState;
    private GameObject smokeParticle;
    public CarIdleState(Car car, CarMovementState movementState, GameObject smokeParticle)
    {
        this.car = car;
        this.movementState = movementState;
        this.smokeParticle = smokeParticle;
        carMovementController = car.GetComponent<CarMovementController>();
        animator = car.GetComponent<Animator>();
        SetPathTiles();
    }
    public void OnEnter()
    {
        if (ControlPathTiles())
        {
            car.StateMachine.ChangeState(movementState);
        }
        else
        {
            animator.SetBool("EngineOn", true);
            smokeParticle.SetActive(true);
            car.StartCoroutine(WaitForValidPath());
        }
    }

    public void OnExit()
    {
        animator.SetBool("EngineOn", false);
        smokeParticle.SetActive(false);
    }

    private void SetPathTiles()
    {
        foreach (CarPathContainer path in carMovementController.ExitPaths)
        {
            pathTiles.Add(path.PathTiles);
        }
    }
    private bool ControlPathTiles()
    {
        foreach (List<Tile> path in pathTiles)
        {
            bool validPath = true;

            foreach (Tile tile in path)
            {
                if (tile.IsBlocked)
                {
                    validPath = false;
                    break;
                }
            }

            if(validPath)
            {
                movementState.SelectedPath = path;
                return true;
            }
        }

        return false;
    }

    private IEnumerator WaitForValidPath()
    {
        bool pathBlocked = true;

        while (pathBlocked)
        {
            yield return null;
            pathBlocked = !ControlPathTiles();
        }

        car.StateMachine.ChangeState(movementState);
    }
}
