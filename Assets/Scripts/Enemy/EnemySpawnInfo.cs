using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Enemy name")]
    private string m_Name;
    public string EnemyName
    {
        get
        {
            return m_Name;
        }
    }

    [SerializeField]
    [Tooltip("Enemy game object")]
    private GameObject m_EnemyGO;
    public GameObject EnemyGO
    {
        get
        {
            return m_EnemyGO;
        }
    }

    [SerializeField]
    [Tooltip("Time until next enemy spawn")]
    private float m_TimeToSpawn;
    public float TimeToNextSpawn
    {
        get
        {
            return m_TimeToSpawn;
        }
    }

    [SerializeField]
    [Tooltip("Time until next enemy spawn")]
    private float m_NumberToSpawn;
    public float NumberToSpawn
    {
        get
        {
            return m_NumberToSpawn;
        }
    }
    #endregion
}
