using UnityEngine;
using Zenject;

public class StickmanInstallerTutorial : MonoInstaller
{
    public Material outlineMaterial;
    public override void InstallBindings()
    {
        Container.Bind<Stickman>().FromComponentInHierarchy().AsSingle();
        Container.Bind<StickmanStateMachine>().AsSingle();

        Container.Bind(typeof(StickmanSelectedDecoratorState), typeof(IStickmanState)).WithId("Stc_Selected").To<StickmanSelectedDecoratorState>().AsSingle();
        Container.Bind<StickmanSelectedState>().AsSingle().WithArguments(outlineMaterial);

        Container.Bind(typeof(StickmanCarControlDecoratorState), typeof(IStickmanState)).WithId("Stc_CarControl").To<StickmanCarControlDecoratorState>().AsSingle();
        Container.Bind<StickmanCarControlState>().AsSingle();

        Container.Bind(typeof(StickmanIdleState), typeof(IStickmanState)).WithId("Stc_Idle").To<StickmanIdleState>().AsSingle();
        Container.Bind(typeof(StickmanMovementState), typeof(IStickmanState)).WithId("Stc_Movement").To<StickmanMovementState>().AsSingle();
        Container.Bind(typeof(StickmanPathFindingState), typeof(IStickmanState)).WithId("Stc_PathFinding").To<StickmanPathFindingState>().AsSingle();
        Container.Bind(typeof(StickmanCarEnterState), typeof(IStickmanState)).WithId("Stc_EnterCar").To<StickmanCarEnterState>().AsSingle();
    }
}
