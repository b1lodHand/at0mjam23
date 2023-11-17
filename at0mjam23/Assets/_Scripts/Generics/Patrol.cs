using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private PatrolState m_state = PatrolState.NotActive;
    [SerializeField] private List<Spot> m_spots = new List<Spot>();
    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private bool m_reverseOnEnd = true;

    // Private.
    private float m_currentTimeWaited = 0f;
    private float m_currentSpotDuration = 0f;
    private float m_distaneToNextSpot = 0f;
    private Vector2 m_moveDirection;
    private Spot m_lastOrCurrentSpot = null;
    private Spot m_nextSpot = null;
    private int m_currentSpotIndex = -1;

    private void Start()
    {
        if (m_spots.Count <= 0) { Deactivate(); return; }
        StartMoving();
    }

    private void FixedUpdate()
    {
        switch (m_state)
        {
            case PatrolState.NotActive:
                m_rb.velocity = Vector3.zero;
                break;
            case PatrolState.Moving:
                Move();
                break;
            case PatrolState.Waiting:
                Wait();
                break;
            default:
                break;
        }
    }

    private void Wait()
    {
        if (m_currentTimeWaited < m_currentSpotDuration) m_currentTimeWaited += Time.deltaTime;
        else StartMoving();
    }

    private void Move()
    {
        m_distaneToNextSpot = Vector2.Distance(m_nextSpot.SpotPosition.position, transform.position);
        if (m_distaneToNextSpot <= .5f) { StartWaiting(); return; }

        var speedMultiplier = 10f;
        m_rb.velocity = m_moveDirection * m_moveSpeed * speedMultiplier * Time.deltaTime;
    }

    private void StartWaiting()
    {
        if(m_nextSpot.SpotDuration <= 0f) { StartMoving(); return; }

        m_currentSpotDuration = m_nextSpot.SpotDuration;
        m_currentTimeWaited = 0f;

        m_state = PatrolState.Waiting;
    }

    private void StartMoving()
    {
        if (m_currentSpotIndex != -1) m_lastOrCurrentSpot = m_spots[m_currentSpotIndex];
        else m_lastOrCurrentSpot = null;

        m_currentSpotIndex++;

        if (m_currentSpotIndex >= m_spots.Count)
        {
            if(!m_reverseOnEnd) { Deactivate(); return; }

            m_spots.Reverse();
            m_currentSpotIndex = -1;
            StartMoving();
            return;
        }

        m_nextSpot = m_spots[m_currentSpotIndex];
        m_moveDirection = (m_nextSpot.SpotPosition.position - transform.position).normalized;

        m_state = PatrolState.Moving;
    }

    public void Deactivate() => m_state = PatrolState.NotActive;

    private void OnDrawGizmosSelected()
    {
        if (m_spots.Count <= 0) return;
        
        Gizmos.color = Color.red;
        for (int i = 0; i < m_spots.Count; i++)
        {
            Gizmos.DrawSphere(m_spots[i].SpotPosition.position, .2f);
            if (i == m_spots.Count - 1) return;
            Gizmos.DrawLine(m_spots[i+1].SpotPosition.position, m_spots[i].SpotPosition.position);
        }
    }

}

[System.Serializable]
public class Spot
{
    // Fields.
    [SerializeField] private Transform m_spotPosition = null;
    [SerializeField] private float m_spotDuration = 0f;

    // Properties.
    public Transform SpotPosition => m_spotPosition;
    public float SpotDuration => m_spotDuration;
}

public enum PatrolState
{
    NotActive = 0,
    Moving = 1,
    Waiting  = 2,
}