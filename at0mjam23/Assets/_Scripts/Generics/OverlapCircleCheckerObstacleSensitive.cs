using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircleCheckerObstacleSensitive : MonoBehaviour
{
    // Fields.
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private LayerMask m_obstacleMask;
    [SerializeField] private float m_checkRadius;
    [SerializeField] private List<Transform> m_result = new List<Transform>();

    // Properties.
    public bool FoundAny => m_result.Count > 0;
    public List<Transform> Result => m_result;

    private void Update()
    {
        Check();
    }

    void Check()
    {
        m_result.Clear();

        var interactablesInRange = Physics2D.OverlapCircleAll(transform.position, m_checkRadius, m_targetMask);
        if (interactablesInRange.Length == 0) return;

        for (int i = 0; i < interactablesInRange.Length; i++)
        {
            var target = interactablesInRange[i];
            if (Physics2D.Raycast(transform.position, (target.transform.position - transform.position).normalized,
                Vector2.Distance(transform.position, target.transform.position), m_obstacleMask)) continue;

            m_result.Add(target.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_checkRadius);

        if(m_result.Count < 0)

        Gizmos.color = Color.white;
    }
}
