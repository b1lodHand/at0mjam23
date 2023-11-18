using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverlapCircleCheck : MonoBehaviour
{
    // Fields.
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private float m_radius = 1f;
    [SerializeField] private List<Transform> m_result = new List<Transform>();

    // Properties.
    public bool FoundAny => m_result.Count > 0;
    public List<Transform> Result => m_result;

    private void FixedUpdate()
    {
        Check();
    }

    void Check()
    {
        m_result.Clear();

        var found = Physics2D.OverlapCircleAll(transform.position, m_radius, m_layerMask).ToList();
        found.ForEach(c =>
        {
            m_result.Add(c.transform);
        });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_radius);
    }
}
