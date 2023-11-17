using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    // Fields.
    [SerializeField] private List<TriggerTarget> m_targets = new List<TriggerTarget>();
    [SerializeField] private bool m_triggered = false;
    [SerializeField] private bool m_reusable = false;
    [SerializeField] private UnityEvent m_onTrigger;

    public void GetTriggered(IActivator activator)
    {
        if(m_triggered)
        {
            if (m_reusable) m_triggered = false;
            else return;
        }

        m_triggered = true;
        if (m_targets.Count <= 0) return;
        m_targets.ForEach(t => t.Activate(this));

        m_onTrigger?.Invoke();
    }
}
