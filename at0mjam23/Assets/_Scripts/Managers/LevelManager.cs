using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : PersistentSingleton<LevelManager>
{
    // Fields.
    [SerializeField] private List<Level> m_levels = new List<Level>();
    [SerializeField] private Player m_currentPlayer;

    public bool StartLevel(int ID)
    {
        var targetLevel = m_levels.Find(l => l.LevelID == ID);
        if (targetLevel == null) return false;

        //GameManager.SetActiveCamera(targetLevel.LevelCamera);
        return true;
    }
}

[System.Serializable]
public class Level
{
    // Public.
    public int LevelID;
    public CinemachineVirtualCamera LevelCamera;
    public Transform LevelCheckpoint;
}