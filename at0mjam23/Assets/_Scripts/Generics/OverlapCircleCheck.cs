using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircleCheck : MonoBehaviour
{
    // Fields.
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private float m_radius = 1f;
    [SerializeField] private bool m_check = false;

    // Properties.
    public bool Result => m_check;

    private void FixedUpdate()
    {
        m_check = Physics2D.OverlapCircleAll(transform.position, m_radius, m_layerMask).Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_radius);
    }
}
