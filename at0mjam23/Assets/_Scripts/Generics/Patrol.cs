using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PatrolType
{
    Rigidbody,
    Transform
}

public class Patrol : MonoBehaviour, IPatrol
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private PatrolState m_state = PatrolState.NotActive;
    [SerializeField] private List<Spot> m_spots = new List<Spot>();
    [SerializeField] private PatrolType m_type = PatrolType.Rigidbody;
    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private bool m_reverseOnEnd = true;
    [SerializeField] private bool m_stopWhileWaiting = false;

    // Private.
    private float m_currentTimeWaited = 0f;
    private float m_currentSpotDuration = 0f;
    private float m_distaneToNextSpot = 0f;
    private Vector2 m_moveDirection;
    private Spot m_nextSpot = null;
    private int m_currentSpotIndex = -1;
    private bool m_paused = false;
    private PatrolState m_stateBeforePause;

    // Properties.
    public event Action OnWaitingEnds;

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
                if(m_type == PatrolType.Rigidbody) m_rb.velocity = Vector3.zero;
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
        else
        {
            OnWaitingEnds?.Invoke();
            StartMoving();
        }
    }

    private void Move()
    {
        if (m_paused) return;

        m_distaneToNextSpot = Vector2.Distance(m_nextSpot.SpotPosition.position, transform.position);
        if (m_distaneToNextSpot <= .1f) { StartWaiting(); return; }

        var speedMultiplier = 10f;
        if(m_type == PatrolType.Rigidbody) m_rb.velocity = m_moveDirection * m_moveSpeed * speedMultiplier * Time.deltaTime;
        else if (m_type == PatrolType.Transform) transform.position = Vector2.MoveTowards(transform.position, m_nextSpot.SpotPosition.position,
            m_moveSpeed * speedMultiplier * Time.deltaTime);
    }

    private void StartWaiting()
    {
        if(m_paused) return;
        if(m_stopWhileWaiting) m_rb.velocity = Vector3.zero;

        if (m_nextSpot.SpotDuration <= 0f) { StartMoving(); return; }

        m_currentSpotDuration = m_nextSpot.SpotDuration;
        m_currentTimeWaited = 0f;

        m_state = PatrolState.Waiting;
    }

    private void StartMoving()
    {
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
    public void Activate() => StartMoving();

    public PatrolState GetState() => m_state;

    public void AppendOnWaitingEnds(Action actionWillBeAppended)
    {
        OnWaitingEnds += actionWillBeAppended;
    }

    private void OnDrawGizmos()
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

    public void Pause()
    {
        m_paused = true;
        m_stateBeforePause = m_state;
    }

    public void Resume()
    {
        m_paused = false;
        m_state = m_stateBeforePause;
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