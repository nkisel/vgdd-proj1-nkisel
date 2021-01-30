using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackInfo
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Name of this attack.")]
    private string m_Name;
    public string AttackName
    {
        get
        {
            return m_Name;
        }
    }

    [SerializeField]
    [Tooltip("Ability activation button")]
    private string m_Button;
    public string Button
    {
        get
        {
            return m_Button;
        }
    }

    [SerializeField]
    [Tooltip("The trigger string to activate this attack in the animator")]
    private string m_TriggerName;
    public string TriggerName
    {
        get
        {
            return m_TriggerName;
        }
    }

    [SerializeField]
    [Tooltip("The prefab of the game object representing the ability")]
    private GameObject m_AbilityGO;
    public GameObject AbilityGO
    {
        get
        {
            return m_AbilityGO;
        }
    }

    [SerializeField]
    [Tooltip("Where to spawn the ability game object with respect to the player")]
    private Vector3 m_offset;
    public Vector3 Offset
    {
        get
        {
            return m_offset;
        }
    }

    [SerializeField]
    [Tooltip("Time between button press and ability activation")]
    private float m_windup;
    public float Windup
    {
        get
        {
            return m_windup;
        }
    }

    [SerializeField]
    [Tooltip("Freeze player actions for this amount of time")]
    private float m_FrozenTime;
    public float FrozenTime
    {
        get
        {
            return m_FrozenTime;
        }
    }

    [SerializeField]
    [Tooltip("Amount of time to wait before ability becomes available again")]
    private float m_Cooldown;

    [SerializeField]
    [Tooltip("Amount of health this ability costs")]
    private int m_HealthCost;
    public int HealthCost
    {
        get
        {
            return m_HealthCost;
        }
    }

    [SerializeField]
    [Tooltip("Amount of health this ability costs")]
    private Color m_Color;
    public Color AbilityColor
    {
        get
        {
            return m_Color;
        }
    }

    #endregion

    #region Public Variables

    public float Cooldown
    {
        get;
        set;
    }

    #endregion

    #region Cooldown Methods
    public void ResetCooldown()
    {
        Cooldown = m_Cooldown;
    }

    public bool IsReady()
    {
        return Cooldown <= 0;
    }
    #endregion
}
