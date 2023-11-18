using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalGenerator m_generator;
    [SerializeField] private Transform m_up, m_down;
    [SerializeField] private bool m_pressed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        m_generator.SetSignal(true);
        m_pressed = true;
        m_up.gameObject.SetActive(false);
        m_down.gameObject.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        m_generator.SetSignal(false);
        m_pressed = false;
        m_up.gameObject.SetActive(true);
        m_down.gameObject.SetActive(false);
    }
}
