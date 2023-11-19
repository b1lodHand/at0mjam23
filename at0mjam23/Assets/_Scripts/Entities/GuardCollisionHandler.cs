using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCollisionHandler : MonoBehaviour, IBreakable
{

    [SerializeField] private Guard m_guardScript;

    public bool Break(float duration) => m_guardScript.Break(duration);

    public Transform GetTransform() => m_guardScript.GetTransform();

    public bool IsBroken() => m_guardScript.IsBroken();

    public void Recover() => m_guardScript.Recover();
}
