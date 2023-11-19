using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private OverlapCircleCheck m_groundCheck;
    [SerializeField] private AudioSource m_movementAudioSource;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeedGrounded;
    [SerializeField] private float m_moveSpeedInAir;
    [SerializeField] private float m_groundedDrag, m_inAirDrag;
    [SerializeField] private bool m_isFacingRight = true;

    [Header("Jump")]
    [SerializeField] private float m_ascendingGravityScale;
    [SerializeField] private float m_descendingGravityScale;
    [SerializeField] private float m_groundedGravityScale;
    [SerializeField] private float m_maxJumpButtonHold = .3f;
    [SerializeField] private float m_jumpHeight;
    //[SerializeField] private int m_maxJumps = 1;

    // Private.
    // movement:
    float m_moveInput;
    float m_jumpButtonHold = 0f;
    //int m_currentJumps = 0;
    bool m_isGroundedLastFrame;
    bool m_isGrounded;
    bool m_jumpButtonPressed;
    bool m_hasContactWithMovingGround;
    bool m_anyInput = false;
    bool m_anyInputLastFrame = false;

    // Properties.
    private float m_moveSpeed => m_isGrounded ? m_moveSpeedGrounded : m_moveSpeedInAir;
    private bool m_isOnMovingGround => m_isGrounded && m_hasContactWithMovingGround;
    public bool AnyInput => m_anyInput;

    private void Update()
    {
        m_anyInputLastFrame = m_anyInput;
        m_anyInput = m_moveInput != 0f || m_jumpButtonPressed;

        if ((!m_anyInputLastFrame) && m_anyInput) m_movementAudioSource.Play();
        else if (m_anyInputLastFrame && !m_anyInput) m_movementAudioSource.Pause();

        m_isGroundedLastFrame = m_isGrounded;
        m_isGrounded = m_groundCheck.FoundAny;

        //if (m_isGrounded) m_currentJumps = m_maxJumps;
        m_rb.drag = m_isGrounded ? m_groundedDrag : m_inAirDrag;
        if (!m_isGrounded) m_rb.gravityScale = m_rb.velocity.y < 0 ? m_descendingGravityScale : m_ascendingGravityScale;
        else m_rb.gravityScale = m_groundedGravityScale;

        GetInput();
        if ((m_moveInput < 0f && m_isFacingRight) || (m_moveInput > 0f && !m_isFacingRight))
        {
            m_isFacingRight = !m_isFacingRight;
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void FixedUpdate()
    {
        Move();
        ClampSpeed();
    }

    void GetInput()
    {
        m_moveInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        {
            m_jumpButtonPressed = true;
            m_jumpButtonHold = 0f;
            //m_currentJumps--;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) m_jumpButtonPressed = false;
    }

    void Move()
    {
        var speedMultiplier = 10f;
        m_rb.AddForce(Vector2.right * m_moveInput * m_moveSpeed * speedMultiplier * Time.deltaTime, ForceMode2D.Force);

        // Jump logic.
        if (!m_jumpButtonPressed) return;
        if (m_jumpButtonHold < m_maxJumpButtonHold)
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpHeight);
            m_jumpButtonHold += Time.deltaTime;
        }
        else
        {
            m_jumpButtonPressed = false;
        }
    }

    void ClampSpeed()
    {
        var velocity = m_rb.velocity;
        if (velocity.x > m_moveSpeed) m_rb.velocity = new Vector2(m_moveSpeed, velocity.y);
    }
} 
