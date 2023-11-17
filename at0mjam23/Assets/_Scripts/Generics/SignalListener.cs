using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalListener : MonoBehaviour
{
    // Properties.
    public event Action OnSignalReceive;

    public void GetSignal(SignalGenerator source)
    {
        if (source == null) return;
        OnSignalReceive?.Invoke();
    }
}
