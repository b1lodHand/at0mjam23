using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalListener m_listener;
    [SerializeField] private Transform m_doorObject;
    [SerializeField] private Transform m_closedPosition, m_openedPosition;
    [SerializeField] private float m_openingSpeed, m_closingSpeed;
    [SerializeField] private bool m_open = false;

    // Private.
    private Tween m_currentTween;

    private void Awake()
    {
        m_listener.OnSignalChanged += Switch;
    }

    private void Switch()
    {
        if (m_open)
        {
            Close();
            return;
        }
        else
        {
            Open();
            return;
        }
    }

    private void Open()
    {
        m_open = true;
        if (m_currentTween.IsActive()) m_currentTween.Kill();
        m_currentTween = m_doorObject.DOMove(m_openedPosition.position, m_openingSpeed).SetEase(Ease.OutSine);
    }

    private void Close()
    {
        m_open = false;
        if (m_currentTween.IsActive()) m_currentTween.Kill();
        m_currentTween = m_doorObject.DOMove(m_closedPosition.position, m_closingSpeed).SetEase(Ease.OutSine);
    }
}
