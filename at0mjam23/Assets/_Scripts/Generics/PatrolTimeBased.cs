using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTimeBased : MonoBehaviour, IPatrol
{
    // Fields.
    [SerializeField] private PatrolState m_state = PatrolState.NotActive;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private float m_waitTime;
    [SerializeField] private float m_moveTime;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private Vector2 m_moveDirection;

    // Private.
    private float m_currentTimeMoving;
    private float m_currentTimeWaiting;
    private bool m_paused = false;
    private PatrolState m_stateBeforePause;

    // Properties.
    public event Action OnWaitingEnds;

    private void Start()
    {
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

    private void StartMoving()
    {
        m_currentTimeMoving = 0f;
        m_state = PatrolState.Moving;
    }

    private void StartWaiting()
    {
        m_currentTimeWaiting = 0f;
        m_state = PatrolState.Waiting;
    }

    private void Wait()
    {
        if(m_paused) return;

        if (m_currentTimeWaiting >= m_waitTime)
        {
            OnWaitingEnds?.Invoke();
            StartMoving();
            return;
        }

        else m_currentTimeWaiting += Time.deltaTime;
    }

    private void Move()
    {
        if(m_paused) return;

        if (m_currentTimeMoving >= m_moveTime)
        {
            StartWaiting();
            m_moveDirection *= -1;
            return;
        }

        else
        {
            m_currentTimeMoving += Time.deltaTime;

            var speedMultiplier = 10f;
            m_rb.velocity = m_moveDirection * m_moveSpeed * speedMultiplier * Time.deltaTime;
        }
    }

    public void Activate()
    {
        StartMoving();
    }

    public void Deactivate()
    {
        m_paused = false;
        m_state = PatrolState.NotActive;
    }

    public void Pause()
    {
        m_paused = true;
        m_stateBeforePause = m_state;
        m_state = PatrolState.NotActive;
    }

    public void Resume()
    {
        m_paused = false;
        m_state = m_stateBeforePause;
    }

    public PatrolState GetState() => m_state;

    public void AppendOnWaitingEnds(Action actionWillBeAppended)
    {
        OnWaitingEnds += actionWillBeAppended;
    }
}
