using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class RotationCommand : TransformCommandBase
{
    [SerializeField] private Vector3 eulerRotation;
    public override async void Execute(GameObject source, CancellationToken token)
    {
        float t = 0;
        Transform sourceTransform = source.transform;

        Vector3 startRotEuler = sourceTransform.rotation.eulerAngles;
        Vector3 targetRotEuler = startRotEuler + eulerRotation;

        Quaternion startRot = Quaternion.Euler(startRotEuler);
        Quaternion targetRot = Quaternion.Euler(targetRotEuler);
        onEnter?.Invoke();

        while (t < 1 && !token.IsCancellationRequested)
        {
            sourceTransform.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            t += Time.deltaTime / completeTime;
            await Task.Yield();
        }

        if (token.IsCancellationRequested)
        {
            return;
        }

        onExit?.Invoke();
        sourceTransform.transform.rotation = targetRot;
    }

    #region Editor Methods

#if UNITY_EDITOR
    public override async void ExecuteInEditor(GameObject source, CancellationToken token)
    {
        float t = 0;
        Transform sourceTransform = source.transform;

        Vector3 startRotEuler = sourceTransform.rotation.eulerAngles;
        Vector3 targetRotEuler = startRotEuler + eulerRotation;

        Quaternion startRot = Quaternion.Euler(startRotEuler);
        Quaternion targetRot = Quaternion.Euler(targetRotEuler);

        while (t < 1 && !token.IsCancellationRequested)
        {
            sourceTransform.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            t += Time.deltaTime / completeTime;
            await Task.Yield();
        }

        if (token.IsCancellationRequested)
        {
            return;
        }
        sourceTransform.transform.rotation = targetRot;
    }
#endif
    #endregion
}
