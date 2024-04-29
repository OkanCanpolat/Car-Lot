using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<StickmanSelectedSignal>();
        Container.DeclareSignal<CarSelectedSignal>();
    }
}
public class StickmanSelectedSignal { public Stickman Stickman; }
public class CarSelectedSignal { }