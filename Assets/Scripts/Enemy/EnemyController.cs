using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Maximum health")]
    protected int m_MaxHealth;

    [SerializeField]
    [Tooltip("How fast the enemy can move")]
    protected int m_Speed;

    [SerializeField]
    [Tooltip("Damage dealt per frame")]
    protected float m_Damage;

    [SerializeField]
    [Tooltip("The explosion that occurs when this enemy dies")]
    protected GameObject m_Explosion;

    [SerializeField]
    [Tooltip("The probability that this enemy drops a health pill")]
    protected float m_DropChance;

    [SerializeField]
    [Tooltip("The type of drop")]
    protected GameObject m_DropItem;

    [SerializeField]
    [Tooltip("Points awarded for defeating this enemy")]
    protected int m_Points;
    #endregion

    #region Private Variables
    protected float p_curHealth;
    #endregion

    #region Cached Components
    protected Rigidbody cc_Rb;
    #endregion

    #region Cached References
    protected Transform cr_Player;
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
