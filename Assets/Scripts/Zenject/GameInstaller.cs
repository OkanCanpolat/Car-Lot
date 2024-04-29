using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private SaveData saveData;
    [SerializeField] private LevelData levelData;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<CarExitSginal>();
        Container.DeclareSignal<LevelFinishSignal>();
       
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<SaveData>().FromInstance(saveData).AsSingle();
        Container.Bind<LevelData>().FromInstance(levelData).AsSingle();

        Container.Bind<TileStateMachine>().AsSingle();
        Container.Bind<GridSystem>().FromComponentInHierarchy().AsSingle();

        Container.Bind(typeof(ITileState), typeof(TileIdleState)).WithId("T_Idle").To<TileIdleState>().AsSingle();
        Container.Bind(typeof(ITileState), typeof(TileAlertState)).WithId("T_Alert").To<TileAlertState>().AsSingle();

        Container.Bind<Stickman>().FromComponentsInHierarchy(s => s.gameObject.activeSelf, false).AsSingle();

        Container.Bind<IPathFinder>().To<AStarPathFinder>().AsSingle();
    }
}
public class CarExitSginal { }
public class LevelFinishSignal { }


