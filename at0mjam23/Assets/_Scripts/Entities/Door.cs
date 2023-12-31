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
    [SerializeField] private AudioClip m_openClip;

    // Private.
    private Tween m_currentTween;

    private void Awake()
    {
        m_listener.OnPositiveSignalReceived += Open;
        m_listener.OnNegativeSignalReceived += Close;
        if (m_open) Open();
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
        if (m_open) return;

        m_open = true;
        if (m_currentTween.IsActive()) m_currentTween.Kill();
        m_currentTween = m_doorObject.DOMove(m_openedPosition.position, m_openingSpeed).SetEase(Ease.OutSine);
        AudioSource.PlayClipAtPoint(m_openClip, transform.position, .5f);
    }

    private void Close()
    {
        if(!m_open) return;

        m_open = false;
        if (m_currentTween.IsActive()) m_currentTween.Kill();
        m_currentTween = m_doorObject.DOMove(m_closedPosition.position, m_closingSpeed).SetEase(Ease.OutSine);
        AudioSource.PlayClipAtPoint(m_openClip, transform.position, .5f);
    }
}
