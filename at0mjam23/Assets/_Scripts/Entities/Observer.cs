using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Observer : MonoBehaviour, IBreakable
{
    // Fields.
    [SerializeField] private VisionCone m_visionCone;
    [SerializeField] private Light2D m_visionLight;
    [SerializeField] private Transform m_headLight;
    [SerializeField] private GameObject m_stunEffect;
    [SerializeField] private bool m_breakable = true;
    [SerializeField] private bool m_mutable = true;
    [SerializeField] private bool m_startBroken = false;

    [SerializeField] private AudioClip m_breakClip;
    [SerializeField] private AudioClip m_recoverClip;

    // Private
    private bool m_isBroken = false;
    private float m_lastLightIntensity;
    private SignalListener m_listener;

    private void Start()
    {
        if (TryGetComponent(out m_listener))
        {
            m_listener.OnPositiveSignalReceived += DisableWithSignal;
            m_listener.OnNegativeSignalReceived += Recover;
        }

        if (m_startBroken) Break(-1);
    }

    private void Update()
    {
        Search();
    }

    void Search()
    {
        if(m_visionCone.VisibleTargets.Count == 0) return;

        var containsPlayer = m_visionCone.VisibleTargets.Any(t =>
        {
            if (!t.TryGetComponent(out Player player)) return false;
            if (player.IsHidden) return false;

            return true;
        });

        var containsAnyBrokenGuard = m_visionCone.VisibleTargets.Any(t =>
        {
            if (!t.TryGetComponent(out IBreakable breakable)) return false;
            if (!breakable.IsBroken()) return false;

            return true;
        });

        if (containsPlayer || containsAnyBrokenGuard) GameManager.Instance.KillPlayer();
    }

    public bool Break(float duration)
    {
        if (((!m_breakable) && (!m_mutable)) || m_isBroken) return false;
        m_isBroken = true;
        m_lastLightIntensity = m_visionLight.intensity;
        m_visionCone.Deactivate();
        m_headLight.gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(m_breakClip, transform.position, .5f);
        m_stunEffect.SetActive(true);
        DOVirtual.Float(m_lastLightIntensity, .03f, .5f, f => m_visionLight.intensity = f).SetEase(Ease.OutBounce);
        if (duration != -1) Invoke("Recover", duration);
        return true;
    }

    public void Recover()
    {
        if(!m_isBroken) return;

        m_isBroken = false;
        m_headLight.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(m_recoverClip, transform.position, .5f);
        m_stunEffect.SetActive(false);
        DOVirtual.Float(.03f, m_lastLightIntensity, .5f, f => m_visionLight.intensity = f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            m_visionCone.Activate();
        });
    }

    public void DisableWithSignal()
    {
        if (!m_mutable) return;
        Break(-1);
    }

    public Transform GetTransform() => transform;

    public bool IsBroken() => m_isBroken;
}
