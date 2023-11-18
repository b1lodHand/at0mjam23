using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalListener : MonoBehaviour
{
    // Fields.
    [SerializeField] private bool m_negateSignals = false;

    // Private.
    private SignalGate m_controller;
    private bool m_lastSignalState;

    // Properties.
    public event Action OnPositiveSignalReceived;
    public event Action OnNegativeSignalReceived;
    public event Action OnSignalChanged;
    public event Action OnSignalChangedToPositive;
    public event Action OnSignalChangedToNegative;
    public bool NegateIncomingSignals => m_negateSignals;

    private void Start()
    {
        if(m_controller == null) this.enabled = false;
    }

    public void ListenToSignal(bool signalState)
    {
        if (m_negateSignals) signalState = !signalState;

        if (signalState) OnPositiveSignalReceived?.Invoke();
        else OnNegativeSignalReceived?.Invoke();
        if (m_lastSignalState != signalState)
        {
            OnSignalChanged?.Invoke();
            if (signalState) OnSignalChangedToPositive?.Invoke();
            else OnSignalChangedToNegative?.Invoke();
        }

        m_lastSignalState = signalState;
    }

    public void SetController(SignalGate controller)
    {
        m_controller = controller;
    }
}
