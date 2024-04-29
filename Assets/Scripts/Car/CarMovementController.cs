using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Threading;
using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CarPathContainer
{
    [Tooltip("These tiles must not be blocked for car to move")]
    public List<Tile> PathTiles;
    [SerializeReference] public List<TransformCommandBase> Commands;
}

[RequireComponent(typeof(Car))]
public class CarMovementController : MonoBehaviour
{
    [SerializeField] private List<CarPathContainer> exitPaths;
    private SignalBus signalBus;
    public List<CarPathContainer> ExitPaths => exitPaths;
    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }

#if UNITY_EDITOR
    public bool EditorScripting = false;
    public CancellationTokenSource CancellationTokenSource;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
#endif
    public void AddCommond(TransformCommandBase command, int index)
    {
        exitPaths[index].Commands.Add(command);
    }
    public async void RunCommands(List<TransformCommandBase> selectedCommands)
    {
        try
        {
            foreach (TransformCommandBase c in selectedCommands)
            {
                float animationCompleteTime = c.CompleteTime;
                float delayTime = animationCompleteTime * c.NextCommandEntryTime;
                int delayTimeInt = (int)(delayTime * 1000);
                c.Execute(gameObject, GameManager.Token);
                await Task.Delay(delayTimeInt, GameManager.Token);
            }
        }

        catch (OperationCanceledException)
        {
        }
    }

    public void OnExit()
    {
        signalBus.Fire<CarExitSginal>();
    }

#if UNITY_EDITOR
    public async void RunCommandsEditor(int index)
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(GameManager.Token);
        List<TransformCommandBase> selectedCommands = exitPaths[index].Commands;
        try
        {
            foreach (TransformCommandBase c in selectedCommands)
            {
                float animationCompleteTime = c.CompleteTime;
                float delayTime = animationCompleteTime * c.NextCommandEntryTime;
                int delayTimeInt = (int)(delayTime * 1000);
                c.ExecuteInEditor(gameObject, CancellationTokenSource.Token);
                await Task.Delay(delayTimeInt, CancellationTokenSource.Token);
            }
        }

        catch (OperationCanceledException)
        {
        }
        finally
        {
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }
    }
    public void ResetTransform()
    {
        transform.position = lastPosition;
        transform.rotation = lastRotation;
    }
#endif
}
