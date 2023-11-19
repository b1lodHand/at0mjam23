using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporterButton : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalListener m_listener;
    [SerializeField] private Transform m_newTransform;

    private void Start()
    {
        m_listener.OnPositiveSignalReceived += () => GameManager.Instance.TeleportPlayer(m_newTransform.position);
    }
}
