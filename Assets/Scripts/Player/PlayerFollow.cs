using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The player to follow")]
    private Transform m_PlayerTransform;

    [SerializeField]
    [Tooltip("The offset from the player's origin to the camera")]
    private Vector3 m_Offset;

    [SerializeField]
    [Tooltip("Maximum camera rotation speed")]
    private float m_RotationSpeed = 10;
    #endregion

    #region
    private void LateUpdate()
    {
        /* Runs after all physics calculations. */
        Vector3 newPos = m_PlayerTransform.position + m_Offset;
        transform.position = Vector3.Slerp(transform.position, newPos, 1);

        float rotationAmount = m_RotationSpeed * Input.GetAxis("Mouse X");
        transform.RotateAround(m_PlayerTransform.position, Vector3.up, rotationAmount);

        m_Offset = transform.position - m_PlayerTransform.position;
    }
    #endregion
}
