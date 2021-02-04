using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How fast the player will move when running")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("The transform of the camera following the player")]
    private Transform m_CameraTransform;

    [SerializeField]
    [Tooltip("A list of attacks this player can perform")]
    private PlayerAttackInfo[] m_Attacks;

    [SerializeField]
    [Tooltip("Amount of health the player starts with")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("The HUD script")]
    private HUDController m_HUD;
    #endregion

    #region Cached References
    private Animator cr_Anim;
    private Renderer cr_Renderer;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Private Variables
    /* The player's current velocity vector, not including magnitude */
    private Vector2 p_Velocity;

    /* How long the player is unable to attack. */
    private float p_FrozenTimer;

    /* Character's default color. */
    private Color p_DefaultColor;

    /* Current health amount. */
    private float p_Health;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
        cr_Anim = GetComponent<Animator>();
        cr_Renderer = GetComponentInChildren<Renderer>();
        p_DefaultColor = cr_Renderer.material.color;

        p_FrozenTimer = 0;
        p_Health = m_MaxHealth;

        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];
            attack.Cooldown = 0;

            if (attack.Windup > attack.FrozenTime)
            {
                Debug.LogError(attack.AttackName + " has a longer windup time than frozen time.");
            }
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Main Updates
    private void Update()
    {
        if (p_FrozenTimer > 0)
        {
            p_Velocity = Vector2.zero;
            p_FrozenTimer -= Time.deltaTime;
            return;
        }
        else
        {
            p_FrozenTimer = 0;
        }

        /* Ability use timers */
        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];

            if (attack.IsReady())
            {
                if (Input.GetButtonDown(attack.Button))
                {
                    p_FrozenTimer = attack.FrozenTime;
                    StartCoroutine(UseAttack(attack));
                    break;
                }
            } else if (attack.Cooldown > 0)
            {
                attack.Cooldown -= Time.deltaTime;
            } else
            {
                if (Input.GetButtonDown(attack.Button))
                {
                    attack.Cooldown -= 0.5f * Time.deltaTime;
                }
            }

        }

        /* Player inputs */
        float forward = Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");

        /* Updating the animation */
        cr_Anim.SetFloat("Speed", Mathf.Clamp01(Mathf.Abs(forward) + Mathf.Abs(right)));

        /* Establish a "deadzone" for movement */
        float moveThreshold = 0.3f;

        if (forward > -moveThreshold && forward < moveThreshold)
        {
            forward = 0;
        }
        if (right > -moveThreshold && right < moveThreshold)
        {
            right = 0;
        }

        p_Velocity.Set(right, forward);
    }

    private void FixedUpdate()
    {
        /* Update the position of the player.
         transform.forward: where the player's facing right now.
         m_Speed: (editor-defined) maximum player speed.
         p_Velocity.magnitude */
        cc_Rb.MovePosition(cc_Rb.position + (m_Speed * transform.forward * Time.fixedDeltaTime * p_Velocity.magnitude));

        /* Update the rotation of the player. */
        cc_Rb.angularVelocity = Vector3.zero;

        if (p_Velocity.sqrMagnitude > 0)
        {
            float angleToRotCam = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            Vector3 camForward = m_CameraTransform.forward;
            Vector3 newRot = new Vector3(Mathf.Cos(angleToRotCam) * camForward.x - Mathf.Sin(angleToRotCam) * camForward.z, 0,
                Mathf.Sin(angleToRotCam) * camForward.x + Mathf.Cos(angleToRotCam) * camForward.z);
            float theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);
        }
    }
    #endregion

    #region Player Health
    public void DecreaseHealth(float amount)
    {
        p_Health -= amount;
        m_HUD.UpdateHealth(1.0f * p_Health / m_MaxHealth);
        if (p_Health < 0)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void IncreaseHealth(float amount)
    {
        p_Health += amount;
        if (p_Health > m_MaxHealth)
        {
            p_Health = m_MaxHealth;
            m_Speed += 2;
        }
        m_HUD.UpdateHealth(1.0f * p_Health / m_MaxHealth);
    }
    #endregion

    #region Attack Methods
    private IEnumerator UseAttack(PlayerAttackInfo attack)
    {
        cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        cr_Anim.SetTrigger(attack.TriggerName);
        IEnumerator toColor = ChangeColor(attack.AbilityColor, 10);
        StartCoroutine(toColor);
        yield return new WaitForSeconds(attack.Windup);

        Vector3 offset = transform.forward * attack.Offset.z + transform.right * attack.Offset.x + transform.up * attack.Offset.y;
        GameObject go = Instantiate(attack.AbilityGO, transform.position + offset, cc_Rb.rotation);
        go.GetComponent<Ability>().Use(transform.position + offset);
        DecreaseHealth(attack.HealthCost);

        StopCoroutine(toColor);
        StartCoroutine(ChangeColor(p_DefaultColor, 50));
        yield return new WaitForSeconds(attack.Cooldown);

        attack.ResetCooldown();
    }
    #endregion

    #region Misc Methods
    private IEnumerator ChangeColor(Color newColor, float speed)
    {
        Color curColor = cr_Renderer.material.color;
        while (curColor != newColor)
        {
            curColor = Color.Lerp(curColor, newColor, speed / 100);
            cr_Renderer.material.color = curColor;
            yield return null;
        }
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPill"))
        {
            IncreaseHealth(other.GetComponent<HealthPill>().Restore);
            Destroy(other.gameObject);
        }
    }
    #endregion
}
