using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The part of the health that decreases")]
    private RectTransform m_HealthBar;
    #endregion

    #region Private Variables
    private float p_HealthBarFullWidth;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_HealthBarFullWidth = m_HealthBar.sizeDelta.x;
    }
    #endregion

    #region Update Health
    public void UpdateHealth(float percent)
    {
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarFullWidth * percent, m_HealthBar.sizeDelta.y);
    }
    #endregion
}
