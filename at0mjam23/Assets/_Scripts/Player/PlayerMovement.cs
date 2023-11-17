using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_maxSpeed;

    // Private.
    private float m_moveInput;
    private Vector2 m_move;

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
        LimitSpeed();
    }

    private void GetInput()
    {
        m_moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        var speedMultiplier = 10f;
        m_move = new Vector2(m_moveInput * m_moveSpeed * speedMultiplier * Time.deltaTime, 0f);
        m_rb.AddForce(m_move, ForceMode2D.Force);
    }

    private void LimitSpeed()
    {
        var velocity = m_rb.velocity;
        if (velocity.x > m_maxSpeed) m_rb.velocity = new Vector2(m_maxSpeed, velocity.y);
    }
}
