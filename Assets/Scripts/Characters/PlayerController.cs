using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Message;
using UnityEngine.InputSystem;
using System;
using System.Linq;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IMessageReceiver
{
    protected static PlayerController s_Instance;
    public static PlayerController Instance { get { return s_Instance; } }

    public CharacterStats m_CharacterStats;

    public LevelSystem playerLevelSystem;

    public float translationSpeed = 8f; //Vitesse de déplacement max
    public float gravity = 20f;
    public float jumpSpeed = 10f;
    public float idleTimeout = 5f;
    public float minTurnSpeed = 400f;         // How fast Ellen turns when moving at maximum speed.
    public float maxTurnSpeed = 1200f;        // How fast Ellen turns when stationary.
    public bool canAttack;
    public CinemachineFreeLook mainCamera;
    public Transform cameraBrain;
    public MeleeWeapon weapon;
    public AbilityBase[] m_Abilities;
    public float interactableCheckRadius = 1f;
    public Vector3 interactableCheckOffset;
    public LayerMask interactableLayer;
    // Ajouter la gestion du son (RandomAudioPlayer)

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    protected AnimatorStateInfo m_CurrentStateInfo;
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;    // Information about the base layer of the animator from last frame.
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;
    protected bool m_IsGrounded;
    protected bool m_ReadyToJump;
    protected float m_DesiredForwardSpeed;
    protected float m_ForwardSpeed;
    protected float m_VerticalSpeed;
    protected PlayerInput m_Input;
    protected CharacterController m_CharacterController;
    protected Animator m_Animator;
    protected Material m_CurrentWalkingSurface;
    protected Quaternion m_TargetRotation; // Le rotation que le personnage veut atteindre.
    protected float m_AngleDiff; // Angle (degré) entre la rotation actuelle du personnage et sa rotation voulu.
    protected Collider[] m_OverlapResult = new Collider[8]; // Les colliders autour du personnage
    protected bool m_InAttack;
    protected bool m_InCombo;
    protected Damageable m_Damageable;
    protected Renderer[] m_Renderers;
    //Ajouter Checkpoint m_currentCheckpoint;
    protected bool m_Respawning;
    protected float m_IdleTimer;
    protected Vector3 moveDirection;
    protected int movementState = 0;
    protected Collider[] m_OverlapInteractable;
    protected Collider[] m_DisplayedInteractable;
    protected int m_DisplayedInteractableIndex;
    protected int m_InteractableIndex;
    // Paramètres de l'animator
    // WIP
    readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
    readonly int m_HashTimeoutToIdle = Animator.StringToHash("TimeoutToIdle");
    readonly int m_HashGrounded = Animator.StringToHash("Grounded");
    readonly int m_HashNormalAttack = Animator.StringToHash("NormalAttack");
    readonly int m_HashStateTime = Animator.StringToHash("StateTime");
    readonly int m_HashHurtFromX = Animator.StringToHash("HurtFromX");
    readonly int m_HashHurtFromY = Animator.StringToHash("HurtFromY");
    readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
    readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
    readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
    readonly int m_HashWeaponSortie = Animator.StringToHash("WeaponSortie");
    readonly int m_HashMovementState = Animator.StringToHash("MovementState");
    readonly int m_HashIdle = Animator.StringToHash("Idle");
    // States
    readonly int m_HashLocomotion = Animator.StringToHash("Movements");
    readonly int m_HashAirborne = Animator.StringToHash("Airborne");
    readonly int m_HashLanding = Animator.StringToHash("Landing");
    readonly int m_HashCombo1 = Animator.StringToHash("Attack_5Combo_1");
    readonly int m_HashCombo2 = Animator.StringToHash("Attack_5Combo_2");
    readonly int m_HashCombo3 = Animator.StringToHash("Attack_5Combo_3");
    readonly int m_HashCombo4 = Animator.StringToHash("Attack_5Combo_4");
    readonly int m_HashCombo5 = Animator.StringToHash("Attack_5Combo_5");
    // Tags
    readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");

    const float k_AirborneTurnSpeedProportion = 5.4f;
    const float k_GroundedRayDistance = 1f;
    const float k_JumpAbortSpeed = 10f;
    const float k_MinEnemyDotCoeff = 0.2f;
    const float k_InverseOneEighty = 1f / 180f;
    const float k_StickingGravityProportion = 0.3f;
    const float k_GroundAcceleration = 20f;
    const float k_GroundDeceleration = 25f;

    //Stats
    [Header("Player base stats")]
    public float atkFlat;
    public float atkPercent;
    public float critDmg;
    public float critRate;

    public bool isWeaponEquipped = false;
    
    // Events
    [HideInInspector] public static Action<Collider[]> OnDisplayedInteractableChanged;
    [HideInInspector] public static Action<int> OnDisplayedInteractableIndexChanged;
    protected bool IsMoveInput
    {
        get { return !Mathf.Approximately(m_Input.MoveInput.sqrMagnitude, 0f); }
    }

    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }

    // Ajouter Reset

    void Awake()
    {
        m_CharacterStats = new CharacterStats(atkFlat, atkPercent, critRate, critDmg);
        m_Input = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();

        //weapon.SetOwner(gameObject);

        playerLevelSystem = new LevelSystem("playerLevel");

        m_Abilities = GetComponents<AbilityBase>();

        s_Instance = this;
    }

    private void OnEnable()
    {
        SceneLinkedSMB<PlayerController>.Initialise(m_Animator, this);
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);

        m_Damageable.isInvulnerable = true;

        m_Renderers = GetComponentsInChildren<Renderer>();
    }
    // Called automatically by Unity whenever the script is disabled.
    void OnDisable()
    {
        m_Damageable.onDamageMessageReceivers.Remove(this);

        for (int i = 0; i < m_Renderers.Length; ++i)
        {
            m_Renderers[i].enabled = true;
        }
    }
    private void FixedUpdate()
    {

        Debug.Log("AtkFlat : " + m_CharacterStats.GetStat(BaseStat.StatType.AtkFlat).baseValue);
        Debug.Log("AtkFlat after calculation : " + m_CharacterStats.GetStat(BaseStat.StatType.AtkFlat).GetCalculatedStatValue());

        CacheAnimatorState();

        UpdateInputBlocking();

        DetectInteractable();

        if (m_Input.Test)
        {
            Debug.Log("playerController");
            GainExperience(5000000);
            m_Input.Test = false;
        }

        m_InCombo = IsInCombo();

        m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        m_Animator.ResetTrigger(m_HashNormalAttack);

        if (m_Input.EquipWeapon)  m_Animator.SetBool(m_HashWeaponSortie, !m_Animator.GetBool(m_HashWeaponSortie));

        if (m_Input.Attack && canAttack && isWeaponEquipped)
        {
            m_Animator.SetTrigger(m_HashNormalAttack);
        }
        if (IsMoveInput)
        {
            if (m_Input.Run)
            {
                movementState = 3;
                
            }
            else if (m_Input.Walk)
            {
                movementState = 1;
            }
            else
            {
                movementState = 2;
            }
        }
        else
        {
            movementState = 0;
        }
        moveDirection = new Vector3();

        Movements();

        // gerer ability
        bool inputDetected = IsMoveInput || m_Input.Attack || m_Input.JumpInput;
        m_Animator.SetBool(m_HashInputDetected, inputDetected);
        // ajouter la gestion audio
        DisplayWeapon();
        DisplayInteractable();
    }

    void DisplayWeapon()
    {
        bool displayWeapon = !(m_CurrentStateInfo.shortNameHash == m_HashLocomotion) && !(m_NextStateInfo.shortNameHash == m_HashLocomotion);
        if (weapon != null)
        {
            weapon.gameObject.SetActive(displayWeapon);
        }
    }
    
    bool IsCalculatingOrientation()
    {
        bool updateOrientationForLocomotion = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLocomotion || m_NextStateInfo.shortNameHash == m_HashLocomotion;
        updateOrientationForLocomotion |= m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashIdle || m_NextStateInfo.shortNameHash == m_HashIdle;
        return updateOrientationForLocomotion || m_InCombo && !m_InAttack;
    }

    void CacheAnimatorState()
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = m_Animator.IsInTransition(0);
    }
    // Called after the animator state has been cached to determine whether this script should block user input.
    void UpdateInputBlocking()
    {
        bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransitioning;
        inputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;
        m_Input.playerControllerInputBlocked = inputBlocked;
    }

    void DetectInteractable()
    {
        m_OverlapInteractable = Physics.OverlapSphere(transform.position + interactableCheckOffset, interactableCheckRadius, interactableLayer);
        for (int i = 0; i < m_OverlapInteractable.Length; i++)
        {
            Debug.Log(m_OverlapInteractable[i].gameObject.name + " : detecté");
        }
    }

    void DisplayInteractable()
    {
        
        if (m_DisplayedInteractable == null || !Enumerable.SequenceEqual(m_DisplayedInteractable, m_OverlapInteractable))
        {
            Debug.Log("diff");
            m_DisplayedInteractable = m_OverlapInteractable;
            int nbDisplayed = Mathf.Clamp(m_DisplayedInteractable.Length, 0, 3);
            Collider[] displayedInteractable = new Collider[nbDisplayed];
            Array.Copy(m_DisplayedInteractable, 0, displayedInteractable, 0, nbDisplayed);
            OnDisplayedInteractableChanged?.Invoke(displayedInteractable);
        }
        //if (m_DisplayedInteractableIndex != m_InteractableIndex)
        //{
        //    m_DisplayedInteractableIndex = m_InteractableIndex;
        //    OnDisplayedInteractableIndexChanged?.Invoke(m_DisplayedInteractableIndex);
        //}
    }

    public void InteractButton(InputAction.CallbackContext ctx)
    {
        if (ctx.started && GameManager.Instance.IsPlaying)
        {
            if (m_OverlapInteractable.Length != 0)
            {
                m_OverlapInteractable[0].GetComponent<Interactable>().PlayerInteracted(gameObject);
            }
            else
            {
                Debug.Log("Nothing");
            }
        }
    }

    // This is called by an animation event
    public void AttackStart(int throwing = 0)
    {
        weapon.BeginAttack(throwing != 0);
        m_InAttack = true;
    }

    // This is called by an animation event
    public void AttackEnd()
    {
        weapon.EndAttack();
        m_InAttack = false;
    }

    bool IsInCombo()
    {
        bool combo = m_NextStateInfo.shortNameHash == m_HashCombo1 || m_CurrentStateInfo.shortNameHash == m_HashCombo1;
        combo |= m_NextStateInfo.shortNameHash == m_HashCombo2 || m_CurrentStateInfo.shortNameHash == m_HashCombo2;
        combo |= m_NextStateInfo.shortNameHash == m_HashCombo3 || m_CurrentStateInfo.shortNameHash == m_HashCombo3;
        combo |= m_NextStateInfo.shortNameHash == m_HashCombo4 || m_CurrentStateInfo.shortNameHash == m_HashCombo4;
        combo |= m_NextStateInfo.shortNameHash == m_HashCombo5 || m_CurrentStateInfo.shortNameHash == m_HashCombo5;
        return combo;
    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        switch (type)
        {
            case MessageType.DAMAGED:
                {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Damaged(damageData);
                }
                break;
            case MessageType.DEAD:
                {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Die(damageData);
                }
                break;
        }
    }

    void Damaged(Damageable.DamageMessage damageMessage)
    {
        // Set the Hurt parameter of the animator.
        //m_Animator.SetTrigger(m_HashHurt);

        // Find the direction of the damage.
        Vector3 forward = damageMessage.damageSource - transform.position;
        forward.y = 0f;

        Vector3 localHurt = transform.InverseTransformDirection(forward);

        // Set the HurtFromX and HurtFromY parameters of the animator based on the direction of the damage.
        m_Animator.SetFloat(m_HashHurtFromX, localHurt.x);
        m_Animator.SetFloat(m_HashHurtFromY, localHurt.z);

        // Shake the camera.
        //CameraShake.Shake(CameraShake.k_PlayerHitShakeAmount, CameraShake.k_PlayerHitShakeTime);

        //// Play an audio clip of being hurt.
        //if (hurtAudioPlayer != null)
        //{
        //    hurtAudioPlayer.PlayRandomClip();
        //}
    }

    // Called by OnReceiveMessage and by DeathVolumes in the scene.
    public void Die(Damageable.DamageMessage damageMessage)
    {
        //m_Animator.SetTrigger(m_HashDeath);
        //m_ForwardSpeed = 0f;
        //m_VerticalSpeed = 0f;
        //m_Respawning = true;
        //m_Damageable.isInvulnerable = true;
    }

    void Movements()
    {
        Vector2 moveInput = m_Input.MoveInput;
        //Orientation a mettre a part FIX

        if (IsCalculatingOrientation())
        {
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraBrain.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }

        //Déplacement
        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        float speedMultiplier = movementState == 1 ? 0.3f : movementState == 3 ? 1.5f : 1f;
        // Calculate the speed intended by input.
        m_DesiredForwardSpeed = moveInput.magnitude * translationSpeed * speedMultiplier;

        // Determine change to speed based on whether there is currently any move input.
        float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

        // Adjust the forward speed towards the desired speed.
        m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);
        // Set the animator parameter to control what animation is being played.
        m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);

        m_VerticalSpeed = -gravity * k_StickingGravityProportion;

    }

    private void OnAnimatorMove()
    {
        Vector3 movement = new Vector3();
        if (m_CurrentStateInfo.shortNameHash == m_HashLocomotion)
        {
            movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
        }
        else
        {
            // ... raycast into the ground...
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f, -Vector3.up);
            if (Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                // ... and get the movement of the root motion rotated to lie along the plane of the ground.
                movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);

                // Also store the current walking surface so the correct audio is played.
                Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();
                m_CurrentWalkingSurface = groundRenderer ? groundRenderer.sharedMaterial : null;
            }
            else
            {
                // If no ground is hit just get the movement as the root motion.
                // Theoretically this should rarely happen as when grounded the ray should always hit.
                movement = m_Animator.deltaPosition;
                m_CurrentWalkingSurface = null;
            }
        }
        movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;
        m_CharacterController.Move(movement);
        m_IsGrounded = m_CharacterController.isGrounded;
        m_Animator.SetBool(m_HashGrounded, m_IsGrounded);
    }

    public void GainExperience(float xp)
    {
        playerLevelSystem.GainExperience(xp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 255, 5);
        Gizmos.DrawSphere(transform.position + interactableCheckOffset, interactableCheckRadius);
    }
}
