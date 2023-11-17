using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalGenerator m_generator;
    [SerializeField] private bool m_pressed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        m_generator.GenerateSignal(activator);
        m_pressed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        m_generator.GenerateSignal(activator);
        m_pressed = false;
    }
}
