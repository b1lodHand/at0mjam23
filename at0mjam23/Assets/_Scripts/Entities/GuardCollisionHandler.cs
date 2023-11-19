using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCollisionHandler : MonoBehaviour, IBreakable, IActivator
{

    [SerializeField] private Guard m_guardScript;
    public bool StartDistractionBy(DistractorBehaviour source, float distractionDuration) =>
        m_guardScript.StartDistractionBy(source, distractionDuration);

    public bool Break(float duration) => m_guardScript.Break(duration);

    public Transform GetTransform() => m_guardScript.GetTransform();

    public bool IsBroken() => m_guardScript.IsBroken();

    public void Recover() => m_guardScript.Recover();
    public bool IsActive() => m_guardScript.IsBroken();
}
