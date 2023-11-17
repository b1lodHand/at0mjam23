using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Fields.
    [SerializeField] private Trigger m_trigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        m_trigger.GetTriggered(activator);
    }
}
