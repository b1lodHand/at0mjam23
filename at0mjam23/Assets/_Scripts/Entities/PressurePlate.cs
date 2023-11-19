using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalGenerator m_generator;
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Sprite m_pressed, m_notPressed;
    [SerializeField] private AudioClip m_pressClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        if (!activator.IsActive()) return;
        m_generator.SetSignal(true);
        m_renderer.sprite = m_pressed;
        AudioSource.PlayClipAtPoint(m_pressClip, transform.position, .5f);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IActivator activator)) return;
        //if (!activator.IsActive) return;
        m_generator.SetSignal(false);
        m_renderer.sprite = m_notPressed;
        AudioSource.PlayClipAtPoint(m_pressClip, transform.position, .5f);
    }
}
