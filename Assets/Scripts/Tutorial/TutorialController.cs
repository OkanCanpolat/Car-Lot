using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<Collider> blockedColliders;
    [SerializeField] private GameObject tapHereImage;
    [SerializeField] private GameObject stickmanArrowUI;
    [SerializeField] private GameObject carArrowUI;
    private SignalBus signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
        signalBus.Subscribe<StickmanSelectedSignal>(OnStickmanSelect);
        signalBus.Subscribe<CarSelectedSignal>(OnCarSelect);
    }
    private void Awake()
    {
        foreach(Collider collider in blockedColliders)
        {
            collider.enabled = false;
        }
    }
    private void OnStickmanSelect(StickmanSelectedSignal signal)
    {
        stickmanArrowUI.SetActive(false);
        carArrowUI.SetActive(true);
        Collider stickmanCollider = signal.Stickman.CurrentTile.GetComponent<Collider>();
        stickmanCollider.enabled = false;
        blockedColliders.Add(stickmanCollider);
        signalBus.Unsubscribe<StickmanSelectedSignal>(OnStickmanSelect);
    }
    private void OnCarSelect()
    {
        tapHereImage.SetActive(false);
        stickmanArrowUI.SetActive(false);
        carArrowUI.SetActive(false);
        signalBus.Unsubscribe<CarSelectedSignal>(OnCarSelect);

        foreach(Collider col in blockedColliders)
        {
            col.enabled = true;
        }
    }
    private void OnDestroy()
    {
        signalBus.TryUnsubscribe<StickmanSelectedSignal>(OnStickmanSelect);
        signalBus.TryUnsubscribe<CarSelectedSignal>(OnCarSelect);
    }
}
