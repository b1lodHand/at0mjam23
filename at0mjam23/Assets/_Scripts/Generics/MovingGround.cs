using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private SignalListener m_listener;

    // Private.
    private bool m_controlledByListener = false;
    private IPatrol m_patrol;

    private void Start()
    {
        if (!TryGetComponent(out m_patrol)) return;
        if (m_listener == null) return;
        m_controlledByListener = true;
        if (m_controlledByListener) m_patrol.Deactivate();
        m_listener.OnPositiveSignalReceived += () => m_patrol.Resume();
        m_listener.OnNegativeSignalReceived += () => m_patrol.Pause();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player)) player.transform.SetParent(transform, true);
        if (collision.gameObject.TryGetComponent(out CardboardBox box)) box.transform.SetParent(transform, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player)) player.transform.SetParent(null, true);
        if (collision.gameObject.TryGetComponent(out CardboardBox box)) box.transform.SetParent(null, true);
    }
}
