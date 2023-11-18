using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour, IInteractable
{
    // Fields.
    [SerializeField] private SignalGenerator m_generator;
    [SerializeField] private Transform m_pressedLight;

    // Private.
    private bool m_pressed = false;

    public void Interact(params object[] data)
    {
        if (m_pressed) return;
        m_pressed = true;
        m_generator.SetSignal(true);
        m_pressedLight.gameObject.SetActive(true);
    }
}
