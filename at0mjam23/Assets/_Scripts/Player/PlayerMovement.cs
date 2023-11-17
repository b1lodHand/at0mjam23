using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private OverlapCircleCheck m_groundCheck;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeed;

    // Private.
    // movement:
    float m_moveInput;

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
        ClampSpeed();
    }

    void GetInput()
    {
        m_moveInput = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        var speedMultiplier = 10f;
        m_rb.AddForce(Vector2.right * m_moveInput * m_moveSpeed * speedMultiplier * Time.deltaTime, ForceMode2D.Force);
    }

    void ClampSpeed()
    {
        var velocity = m_rb.velocity;
        if (velocity.x > m_moveSpeed) m_rb.velocity = new Vector2(m_moveSpeed, velocity.y);
    }
} 
