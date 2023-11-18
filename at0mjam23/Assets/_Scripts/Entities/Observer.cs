using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Observer : MonoBehaviour, IBreakable
{
    // Fields.
    [SerializeField] private VisionCone m_visionCone;
    [SerializeField] private Light2D m_visionLight;
    [SerializeField] private bool m_breakable = true;

    // Private
    private bool m_isBroken = false;
    private float m_lastLightIntensity;

    public bool Break(float duration)
    {
        if (!m_breakable || m_isBroken) return false;
        m_isBroken = true;
        m_lastLightIntensity = m_visionLight.intensity;
        m_visionCone.Disable();
        DOVirtual.Float(m_lastLightIntensity, .03f, .5f, f => m_visionLight.intensity = f).SetEase(Ease.OutBounce);
        Invoke("Recover", duration);
        return true;
    }

    public void Recover()
    {
        m_isBroken = false;
        DOVirtual.Float(.03f, m_lastLightIntensity, .5f, f => m_visionLight.intensity = f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            m_visionCone.Enable();
        });
    }

    public Transform GetTransform() => transform;

    public bool IsBroken() => m_isBroken;
}
