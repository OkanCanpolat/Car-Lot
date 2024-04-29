using UnityEngine;
using Zenject;

public class CarInstaller : MonoInstaller
{
    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject smokeTrial;
    public override void InstallBindings()
    {
        Container.Bind<Car>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CarStateMachine>().AsSingle();
        Container.Bind<CarIdleState>().AsSingle().WithArguments(smoke);
        Container.Bind<CarMovementState>().AsSingle().WithArguments(smokeTrial);
    }
}