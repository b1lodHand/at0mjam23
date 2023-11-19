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
    [SerializeField] private bool m_breakable = true;

    // Private
    private bool m_isBroken = false;
    private float m_lastLightIntensity;

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

        if (containsPlayer || containsAnyBrokenGuard) Debug.Log("ded");
    }

    public bool Break(float duration)
    {
        if (!m_breakable || m_isBroken) return false;
        m_isBroken = true;
        m_lastLightIntensity = m_visionLight.intensity;
        m_visionCone.Deactivate();
        m_headLight.gameObject.SetActive(false);
        DOVirtual.Float(m_lastLightIntensity, .03f, .5f, f => m_visionLight.intensity = f).SetEase(Ease.OutBounce);
        Invoke("Recover", duration);
        return true;
    }

    public void Recover()
    {
        m_isBroken = false;
        m_headLight.gameObject.SetActive(true);
        DOVirtual.Float(.03f, m_lastLightIntensity, .5f, f => m_visionLight.intensity = f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            m_visionCone.Activate();
        });
    }

    public Transform GetTransform() => transform;

    public bool IsBroken() => m_isBroken;
}
