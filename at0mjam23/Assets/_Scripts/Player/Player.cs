using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IActivator
{
    // Fields.
    [SerializeField] private PlayerBreaker m_breaker;

    public void ActivateBreaker()
    {
        if(!m_breaker.Active) m_breaker.Activate();
    }
}
