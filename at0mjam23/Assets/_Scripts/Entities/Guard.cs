using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Guard : MonoBehaviour, IBreakable
{
    // Static.
    private static readonly int Anim_Break = Animator.StringToHash("Break");
    private static readonly int Anim_Recover = Animator.StringToHash("Recover");

    // Fields.
    [SerializeField] private VisionCone m_visionCone;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private OverlapCircleCheckerObstacleSensitive m_nearCheck;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform m_body;
    [SerializeField] private float m_distractedSpeed;
    [SerializeField] private bool m_isBroken = false;
    [SerializeField] private bool m_isDistracted = false;

    // Private.
    private IPatrol m_patrol;
    private Vector3 m_lastBodyPosition;
    private DistractorBehaviour m_distractor;
    private bool m_isFacingRight = false;

    private void Start()
    {
        if (!TryGetComponent(out m_patrol)) m_patrol = null;
    }

    private void Update()
    {
        // Flip logic.
        if((m_rb.velocity.x < 0f && m_isFacingRight) || (m_rb.velocity.x > 0f && !m_isFacingRight))
        {
            m_isFacingRight = !m_isFacingRight;
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        Search();
        CheckNear();

        // Distraction logic.
        if (!m_isDistracted) return;
        if (Vector2.Distance(m_distractor.transform.position, transform.position) < 5f)
        {
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            return;
        }

        var speedMultiplier = 10f;
        m_rb.velocity = new Vector2((m_distractor.transform.position - transform.position).normalized.x
            * m_distractedSpeed * speedMultiplier * Time.deltaTime, m_rb.velocity.y);
    }

    void Search()
    {
        if (m_visionCone.VisibleTargets.Count == 0) return;

        var containsPlayer = m_visionCone.VisibleTargets.Any(t =>
        {
            if (!t.TryGetComponent(out Player player)) return false;
            if (player.IsHidden) return false;

            return true;
        });

        if (containsPlayer) Debug.Log("ded");
    }

    void CheckNear()
    {
        if (!m_nearCheck.FoundAny) return;
            
        var playerIsNear = m_nearCheck.Result.Any(t =>
        {
            if (!t.TryGetComponent(out PlayerCollisionHandler player)) return false;
            if (player.Player.IsHidden) return false;

            return true;
        });

        if (playerIsNear) Debug.Log("ded");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerCollisionHandler player)) Debug.Log("ded");
    }

    public bool IsBroken() => m_isBroken;

    public bool Break(float duration)
    {
        if(IsBroken()) return false;

        m_isBroken = true;
        m_visionCone.Deactivate();
        if(m_patrol != null) m_patrol.Pause();
        m_animator.CrossFade(Anim_Break, 0.2f, 0);
        Invoke("Recover", duration);
        return true;
    }

    public void Recover()
    {
        m_isBroken = false;
        m_visionCone.Activate();
        m_body.DOLocalMove(m_lastBodyPosition, .2f).OnComplete(() =>
        {
            m_animator.enabled = true;
            m_animator.CrossFade(Anim_Recover, 0.2f, 0);
            if (m_patrol != null) m_patrol.Resume();
        });
    }

    public void DisableAnimator()
    {
        m_lastBodyPosition = m_body.localPosition;
        m_animator.enabled = false;
    }

    public bool StartDistractionBy(DistractorBehaviour source, float distractionDuration)
    {
        if (IsBroken() || source == null || m_isDistracted) return false;

        m_isDistracted = true;
        m_distractor = source;
        if (m_patrol != null) m_patrol.Deactivate();

        Invoke("ReleaseDistraction", distractionDuration);
        return true;
    }

    void ReleaseDistraction()
    {
        m_isDistracted = false;
        m_distractor = null;
        if (m_patrol != null) m_patrol.Activate();
    }

    public Transform GetTransform() => transform;
}
