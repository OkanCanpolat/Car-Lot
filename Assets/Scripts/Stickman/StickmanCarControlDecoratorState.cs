using Zenject;

public class StickmanCarControlDecoratorState : IStickmanState
{
    private SignalBus signalBus;
    [Inject] private StickmanCarControlState carControlState;

    public StickmanCarControlDecoratorState(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }
    public void OnEnter()
    {
        signalBus.Fire<CarSelectedSignal>();
        carControlState.OnEnter();
    }

    public void OnExit()
    {
        carControlState.OnExit();
    }
}
