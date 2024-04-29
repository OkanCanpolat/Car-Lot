using Zenject;

public class StickmanIdleState : IStickmanState
{
    [Inject] private Stickman stickman;
    public void OnEnter()
    {
    }
    public void OnExit()
    {
    }
}
