using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Maximum health")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("How fast the enemy can move")]
    private int m_Speed;

    [SerializeField]
    [Tooltip("Damage dealt per frame")]
    private float m_Damage;

    [SerializeField]
    [Tooltip("The explosion that occurs when this enemy dies")]
    private GameObject m_Explosion;

    [SerializeField]
    [Tooltip("The probability that this enemy drops a health pill")]
    private float m_DropChance;

    [SerializeField]
    [Tooltip("The type of drop")]
    private GameObject m_DropItem;

    [SerializeField]
    [Tooltip("Points awarded for defeating this enemy")]
    private int m_Points;
    #endregion

    #region Private Variables
    private float p_curHealth;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Cached References
    private Transform cr_Player;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_curHealth = m_MaxHealth;
        cc_Rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        cr_Player = FindObjectOfType<PlayerController>().transform;
    }
    #endregion

    #region Main Updates
    private void FixedUpdate()
    {
        Vector3 dir = cr_Player.position - transform.position;
        dir.Normalize();
        cc_Rb.MovePosition(cc_Rb.position + dir * m_Speed * Time.deltaTime);
    }
    #endregion

    #region Collision Methods
    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Player"))
        {
            // DecreaseHealth(1);
            other.GetComponent<PlayerController>().DecreaseHealth(m_Damage);
        }
    }
    #endregion

    #region Health Methods
    public void DecreaseHealth(float amount)
    {
        p_curHealth -= amount;
        if (p_curHealth <= 0)
        {
            ScoreManager.singleton.IncreaseScore(m_Points);
            if (Random.value < m_DropChance)
            {
                Instantiate(m_DropItem, transform.position, Quaternion.identity);
            }
            Instantiate(m_Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
    #endregion
}
