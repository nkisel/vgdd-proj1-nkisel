using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPill : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The amount of health this pill restores")]
    private int m_Restore;
    public int Restore
    {
        get
        {
            return m_Restore;
        }
    }
    #endregion
}
