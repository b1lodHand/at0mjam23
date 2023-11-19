using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Fields.
    [SerializeField] private bool m_used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerCollisionHandler player)) return;
        if (m_used) return;

        GameManager.Instance.SetLastCheckpoint(this);
        m_used = true;
    }
}
