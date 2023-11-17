using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalListener m_listener;
    [SerializeField] private Transform m_doorObject;
    [SerializeField] private Transform m_closedPosition, m_openedPosition;
    [SerializeField] private bool m_open = false;

    private void Awake()
    {
        m_listener.OnSignalReceive += Switch;
    }

    private void Switch()
    {
        if (m_open) Close();
        else Open();
    }

    private void Open()
    {
        m_open = true;
    }

    private void Close()
    {
        m_open = false;
    }
}
