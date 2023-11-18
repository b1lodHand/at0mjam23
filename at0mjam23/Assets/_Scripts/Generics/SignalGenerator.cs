using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalGenerator : MonoBehaviour
{
    // Fields.
    [SerializeField] private bool m_currentState = true;

    // Private.
    private SignalGate m_controller;

    // Properties.
    public Action OnSignalChanged;
    public bool SignalState => m_currentState;

    private void Start()
    {
        if (m_controller == null) this.enabled = false;
    }

    public void SetSignal(bool newState)
    {
        if(m_currentState == newState) return;
        NegateSignal();
    }

    public void NegateSignal()
    {
        m_currentState = !m_currentState;
        OnSignalChanged?.Invoke();
    }
    public void SetController(SignalGate controller)
    {
        m_controller = controller;
    }
}
