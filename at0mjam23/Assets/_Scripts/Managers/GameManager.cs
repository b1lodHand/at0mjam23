using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    // Fields.
    [SerializeField] private Camera m_activeCamera;
    [SerializeField] private Transform m_player;
    [SerializeField] private Checkpoint m_lastCheckpoint;

    // Private.

    // Properties.
    public Camera ActiveCamera => m_activeCamera;

    public void KillPlayer()
    {
        m_player.transform.position = m_lastCheckpoint.transform.position;
    }

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint == null) return;
        m_lastCheckpoint = checkpoint;
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        m_player.transform.position = newPosition;
    }
}