using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    // Fields.
    [Header("Settings")]
    [SerializeField] private float m_viewRadius;
    [SerializeField, Range(1, 64)] private int m_countOfRays;
    [SerializeField, Range(0, 360)] float m_viewAngle;
    [Space, Header("Detection")]
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private LayerMask m_obstacleMask;
    [Space]
    [SerializeField] private List<Transform> m_visibleTargets = new List<Transform>();

    // Properties.
    public float ViewRadius => m_viewRadius;
    public float ViewAngle => m_viewAngle;
    public int CountOfRays => m_countOfRays;
    public List<Transform> VisibleTargets => m_visibleTargets;

    private void Update()
    {
        //FindTargets();
        CastRays();
    }

    void CastRays()
    {
        m_visibleTargets.Clear();
        float stepAngle = ViewAngle / m_countOfRays;

        for (int i = 0; i <= m_countOfRays; i++)
        {
            float angle = transform.eulerAngles.z - ViewAngle / 2 + stepAngle * i;
            var direction = GetDirectionFromAngle(angle, true);

            var target = Physics2D.Raycast(transform.position, direction, m_viewRadius, m_targetMask);
            if (target.transform == null) continue;

            var directionToContactPoint = (target.point - new Vector2(transform.position.x, transform.position.y)).normalized;
            if (Physics2D.Raycast(transform.position, directionToContactPoint, target.distance, m_obstacleMask)) continue;

            Debug.DrawLine(transform.position, transform.position + GetDirectionFromAngle(angle, true) * ViewRadius, Color.cyan);
            if (m_visibleTargets.Contains(target.transform)) continue;

            m_visibleTargets.Add(target.transform);
        }
    }

    public Vector3 GetDirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.z;
        return new Vector3(-Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0f);
    }
}
