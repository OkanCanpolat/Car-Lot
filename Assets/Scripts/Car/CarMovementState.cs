using System.Collections.Generic;
using UnityEngine;

public class CarMovementState : ICarState
{
    private List<Tile> selectedPath;
    private Car car;
    private CarMovementController carMovementController;
    private List<TransformCommandBase> commands;
    private GameObject smokeTrial;

    public List<Tile> SelectedPath { get => selectedPath; set => selectedPath = value; }
    public CarMovementState(Car car, GameObject smokeTrial)
    {
        this.car = car;
        this.smokeTrial = smokeTrial;
        carMovementController = car.GetComponent<CarMovementController>();
    }
    public void OnEnter()
    {
        commands = GetCommands(SelectedPath);
        car.ActivateTiles();
        smokeTrial.SetActive(true);
        carMovementController.RunCommands(commands);
    }
    public void OnExit()
    {
    }
    private List<TransformCommandBase> GetCommands(List<Tile> path)
    {
        foreach (CarPathContainer container in carMovementController.ExitPaths)
        {
            if (container.PathTiles == path) return container.Commands;
        }
        return null;
    }
}
