using UnityEngine;
using Zenject;

public class ExitBarrier : MonoBehaviour
{
    private SignalBus signalBus;
    private Animator animator;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
        animator = GetComponent<Animator>();
        signalBus.Subscribe<CarExitSginal>(Open);
    }

    public void Open()
    {
        animator.SetTrigger("Open");
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<CarExitSginal>(Open);
    }
}
