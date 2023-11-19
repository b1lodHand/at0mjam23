using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    // Fields.
    [SerializeField] private Camera m_activeCamera;

    // Properties.
    public Camera ActiveCamera => m_activeCamera;
}