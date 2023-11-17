using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalGenerator : MonoBehaviour
{
    // Fields.
    [SerializeField] private List<SignalListener> m_listeners = new List<SignalListener>();

    public void GenerateSignal(IActivator source)
    {
        if (source == null) return;
        if (m_listeners.Count == 0) return;
        m_listeners.ForEach(l => l.GetSignal(this));
    }
}
