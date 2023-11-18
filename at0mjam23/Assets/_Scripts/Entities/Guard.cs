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
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform m_body;
    [SerializeField] private bool m_isBroken = false;

    // Private.
    private IPatrol m_patrol;
    private Vector3 m_lastBodyPosition;

    private void Start()
    {
        m_patrol = GetComponent<IPatrol>();
        if (m_patrol != null) m_patrol.AppendOnWaitingEnds(() =>
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        });
    }

    void Search()
    {
        if (m_visionCone.VisibleTargets.Count == 0) return;

        var containsPlayer = m_visionCone.VisibleTargets.Any(t => t.TryGetComponent(out Player _));
        if (containsPlayer) Debug.Log("ded");
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
        if(m_patrol != null) m_patrol.Pause();
        m_animator.CrossFade(Anim_Break, 0.2f, 0);
        Invoke("Recover", duration);
        return true;
    }

    public void Recover()
    {
        m_isBroken = false;
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

    public Transform GetTransform() => transform;
}
