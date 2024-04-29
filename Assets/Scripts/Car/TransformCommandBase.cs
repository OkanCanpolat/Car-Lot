using System;
using UnityEngine;
using System.Threading;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public abstract class TransformCommandBase
{
    [SerializeField] protected UnityEvent onEnter;
    [SerializeField] protected UnityEvent onExit;
    [SerializeField] protected float completeTime;
    [SerializeField, Range(0, 1)] protected float nextCommandEntry;
    public float CompleteTime => completeTime;
    public float NextCommandEntryTime => nextCommandEntry;
    public abstract void Execute(GameObject source, CancellationToken token);

    #region Editor Methods

#if UNITY_EDITOR
    public virtual void ExecuteInEditor(GameObject source, CancellationToken token)
    {

    }
#endif
    #endregion
}
