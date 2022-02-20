using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deferred<T>
{
    private List<Action<T>> resolveActions = new List<Action<T>>();
    private List<Action<Exception>> rejectActions = new List<Action<Exception>>();
    private List<Action> alwaysActions = new List<Action>();

    public T Result { get; private set; }

    public Deferred<T> Rejected(Action<Exception> onRejected)
    {
        rejectActions.Add(onRejected);
        return this;
    }

    public Deferred<T> Done(Action<T> onResolved)
    {
        resolveActions.Add(onResolved);
        return this;
    }

    public Deferred<T> Always(Action always)
    {
        alwaysActions.Add(always);
        return this;
    }

    public void Resolve(T resolveData)
    {
        Result = resolveData;
        foreach (var resolveAction in resolveActions)
        {
            resolveAction.Invoke(Result);
        }
        foreach (var alwaysAction in alwaysActions)
        {
            alwaysAction.Invoke();
        }
    }

    public void Reject(Exception error)
    {
        foreach (var rejectAction in rejectActions)
        {
            rejectAction?.Invoke(error);
        }
        foreach (var alwaysAction in alwaysActions)
        {
            alwaysAction.Invoke();
        }
    }
}
