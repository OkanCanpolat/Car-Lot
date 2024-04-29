using Zenject;

public class StickmanSelectedDecoratorState : IStickmanState
{
    private SignalBus signalBus;
    private Stickman stickman;
    [Inject] StickmanSelectedState selectedState;
    public StickmanSelectedDecoratorState(SignalBus signalBus, Stickman stickman)
    {
        this.signalBus = signalBus;
        this.stickman = stickman;
    }
    public void OnEnter()
    {
        signalBus.Fire(new StickmanSelectedSignal() { Stickman = stickman });
        selectedState.OnEnter();
    }
    public void OnExit()
    {
        selectedState.OnExit();
    }
}
