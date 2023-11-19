using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IActivator
{
    // Fields.
    [SerializeField] private PlayerBreaker m_breaker;
    [SerializeField] private PlayerHandle m_handle;
    [SerializeField] private PlayerMovement m_movement;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private GameObject m_collision;
    [SerializeField] private GameObject m_graphic;
    [SerializeField] private bool m_isHidden = false;
    [SerializeField] private float m_hideDelay = .2f;

    // Private.
    private CardboardBox m_hiddenInside;
    private float m_hideDelayCounter = 0f;

    // Properties.
    public bool IsHidden => m_isHidden;
    public bool HideDelayCompleted => m_hideDelayCounter >= m_hideDelay;

    private void Update()
    {
        if (IsHidden && !HideDelayCompleted) m_hideDelayCounter += Time.deltaTime;
        else if (IsHidden && HideDelayCompleted)
        {
            if (m_movement.AnyInput) Unhide();
            else transform.position = m_hiddenInside.HidePosition.position;
        }
    }

    public void ActivateBreaker()
    {
        if(!m_breaker.Active) m_breaker.Activate();
    }

    public bool HandleDistractor() => m_handle.HandleDistractor();

    public void Hide(CardboardBox hiddenInside)
    {
        if(hiddenInside == null) return;

        m_isHidden = true;
        m_hiddenInside = hiddenInside;
        m_hiddenInside.Close();
        transform.position = m_hiddenInside.HidePosition.position;
        m_rb.isKinematic = true;
        m_rb.simulated = false;
        m_graphic.SetActive(false);
        m_collision.SetActive(false);
        if (m_handle.HandlingDistractor) m_handle.DropDistractor();

        m_hideDelayCounter = 0f;
    }

    public void Unhide()
    {
        if (!IsHidden) return;

        m_isHidden = false;
        transform.position = m_hiddenInside.HidePosition.position;
        m_hiddenInside.Open();
        m_hiddenInside = null;
        m_rb.isKinematic = false;
        m_rb.simulated = true;
        m_graphic.SetActive(true);
        m_collision.SetActive(true);
    }

    public bool IsActive() => true;
}
