using UnityEngine;
using Zenject;

public class StickmanInstaller : MonoInstaller
{
    public Material outlineMaterial;
    public override void InstallBindings()
    {
        Container.Bind<Stickman>().FromComponentInHierarchy().AsSingle();
        Container.Bind<StickmanStateMachine>().AsSingle();

        Container.Bind(typeof(StickmanIdleState), typeof(IStickmanState)).WithId("Stc_Idle").To<StickmanIdleState>().AsSingle();
        Container.Bind(typeof(StickmanSelectedState), typeof(IStickmanState)).WithId("Stc_Selected").To<StickmanSelectedState>().AsSingle().WithArguments(outlineMaterial);
        Container.Bind(typeof(StickmanCarControlState), typeof(IStickmanState)).WithId("Stc_CarControl").To<StickmanCarControlState>().AsSingle();
        Container.Bind(typeof(StickmanMovementState), typeof(IStickmanState)).WithId("Stc_Movement").To<StickmanMovementState>().AsSingle();
        Container.Bind(typeof(StickmanPathFindingState), typeof(IStickmanState)).WithId("Stc_PathFinding").To<StickmanPathFindingState>().AsSingle();
        Container.Bind(typeof(StickmanCarEnterState), typeof(IStickmanState)).WithId("Stc_EnterCar").To<StickmanCarEnterState>().AsSingle();
    }
}