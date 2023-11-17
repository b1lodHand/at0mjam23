using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerTarget : MonoBehaviour
{
    // Fields.
    [SerializeField] private bool m_activated = false;
    [SerializeField] private bool m_switch = true;
    [SerializeField] private UnityEvent m_onActivation, m_onDeactivation;

    public void Activate(Trigger trigger)
    {
        if(m_activated)
        {
            if (m_switch) { Deactivate(trigger); return; }
            else return;
        }
        m_activated = true;

        m_onActivation?.Invoke();
    }

    public void Deactivate(Trigger trigger)
    {
        if (!m_activated)
        {
            if (m_switch) { Activate(trigger); return; }
            else return;
        }
        m_activated = false;

        m_onDeactivation?.Invoke();
    }
}
