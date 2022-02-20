using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat<T>
{
    protected T currentValue;

    public Stat(T originalValue)
    {
        this.originalValue = originalValue;
        currentValue = originalValue;
    }

    public event Action<T, T> onValueChanged;

    public T originalValue { get; private set; }
    public T CurrentValue
    {
        get => currentValue;
        set
        {
            var oldVal = currentValue;
            currentValue = value;
            onValueChanged?.Invoke(oldVal, value);
        }

    }
}
