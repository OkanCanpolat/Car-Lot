using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelFinishController : MonoBehaviour
{
    private int levelStikmanCount;
    private SignalBus signalBus;

    [Inject]
    public void Construct(List<Stickman> levelStickmen, SignalBus signalBus)
    {
        this.signalBus = signalBus;
        levelStikmanCount = levelStickmen.Count;
        signalBus.Subscribe<CarExitSginal>(OnCarExit);
    }

    private void OnCarExit()
    {
        levelStikmanCount--;

        if(levelStikmanCount <= 0)
        {
            signalBus.Fire<LevelFinishSignal>();
        }
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<CarExitSginal>(OnCarExit);
    }
}
