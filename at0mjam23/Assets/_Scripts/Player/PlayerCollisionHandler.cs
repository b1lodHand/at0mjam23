using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    // Fields.
    [SerializeField] private Player m_player;

    // Properties.
    public Player Player => m_player;
}
