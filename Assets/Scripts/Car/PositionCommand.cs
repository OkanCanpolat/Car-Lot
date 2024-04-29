using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class PositionCommand : TransformCommandBase
{
    [SerializeField] private Transform target;

    public override async void Execute(GameObject source, CancellationToken token)
    {
        float t = 0;
        Transform sourceTransform = source.transform;
        Vector3 startPos = sourceTransform.position;
        Vector3 targetPos = target.position;
        onEnter?.Invoke();

        while (t < 1 && !token.IsCancellationRequested)
        {
            sourceTransform.position = Vector3.Lerp(startPos, targetPos, t);
            t += Time.deltaTime / completeTime;
            await Task.Yield();
        }

        if (token.IsCancellationRequested)
        {
            return;
        }
        onExit?.Invoke();
        sourceTransform.position = targetPos;
    }

    #region Editor Methods

#if UNITY_EDITOR
    public override async void ExecuteInEditor(GameObject source, CancellationToken token)
    {
        float t = 0;
        Transform sourceTransform = source.transform;
        Vector3 startPos = sourceTransform.position;
        Vector3 targetPos = target.position;

        while (t < 1 && !token.IsCancellationRequested)
        {
            sourceTransform.position = Vector3.Lerp(startPos, targetPos, t);
            t += Time.deltaTime / completeTime;
            await Task.Yield();
        }

        if (token.IsCancellationRequested)
        {
            return;
        }
        sourceTransform.position = targetPos;
    }
#endif
    #endregion

}
