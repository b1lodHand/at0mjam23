using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerBullet : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private AudioClip m_splash;

    // Private.
    private PlayerBreaker m_sender;
    private float m_breakDuration;
    private bool m_initialized = false;

    //private void Start()
    //{
    //    if(!m_initialized) Destroy(gameObject);
    //}

    public void Init(PlayerBreaker sender, float breakDuration, Vector2 velocity)
    {
        m_initialized = true;
        m_sender = sender;
        m_breakDuration = breakDuration;
        m_rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_initialized) return;
        AudioSource.PlayClipAtPoint(m_splash, transform.position, .5f);

        if (collision.TryGetComponent(out IBreakable breakable))
        {
            if (breakable.Break(m_breakDuration)) m_sender.BreakCallback(breakable);
            m_rb.isKinematic = true;
            m_rb.simulated = false;
        }

        Destroy(gameObject);
    }
}
