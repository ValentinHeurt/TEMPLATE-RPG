using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using Cinemachine;


public class PlayerInput : MonoBehaviour
{
    [SerializeField] GameObject m_Inventory;
    [SerializeField] GameObject m_EquipmentSheet;
    [SerializeField] GameObject m_QuestWindow;
    public static PlayerInput Instance
    {
        get { return s_Instance; }
    }

    protected static PlayerInput s_Instance;

    [HideInInspector]
    public bool playerControllerInputBlocked;

    protected Vector2 m_Movement;
    protected Vector2 m_Camera;
    protected bool m_Jump;
    protected bool m_Attack;
    protected bool m_Pause;
    protected bool m_Aim;
    protected bool m_Ability1;
    protected bool m_Ability2;
    protected bool m_EquipWeapon;
    protected bool m_ExternalInputBlocked;
    protected bool m_Run = false;
    protected bool m_Walk;
    protected bool m_Test;

    [SerializeField]
    private float mouseSensitivity = 1;

    public Vector2 MoveInput
    {
        get
        {
            if (playerControllerInputBlocked || m_ExternalInputBlocked || !GameManager.Instance.IsPlaying)
                return Vector2.zero;
            return m_Movement;
        }
    }

    public Vector2 CameraInput
    {
        get
        {
            if (playerControllerInputBlocked || m_ExternalInputBlocked || !GameManager.Instance.IsPlaying)
                return Vector2.zero;
            return m_Camera * mouseSensitivity;
        }
    }
    public bool Test
    {
        get { return m_Test; }
        set { m_Test = value; }
    }

    public bool JumpInput
    {
        get { return m_Jump && !playerControllerInputBlocked && !m_ExternalInputBlocked && GameManager.Instance.IsPlaying; }
    }

    public bool Attack
    {
        get { return m_Attack && !playerControllerInputBlocked && !m_ExternalInputBlocked && GameManager.Instance.IsPlaying; ; }
    }
    public bool Aim
    {
        get { return m_Aim && !playerControllerInputBlocked && !m_ExternalInputBlocked && GameManager.Instance.IsPlaying; ; }
    }
    public bool Ability1
    {
        get { return m_Ability1 && !playerControllerInputBlocked && !m_ExternalInputBlocked && GameManager.Instance.IsPlaying; ; }
    }
    public bool Ability2
    {
        get { return m_Ability2 && !playerControllerInputBlocked && !m_ExternalInputBlocked && GameManager.Instance.IsPlaying; ; }
    }

    public bool Pause
    {
        get { return m_Pause; }
    }

    public bool Run
    {
        get { return m_Run; }
    }

    public bool Walk
    {
        get { return m_Walk; }
    }

    public bool EquipWeapon
    {
        get { return m_EquipWeapon && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

    WaitForSeconds m_AttackInputWait;
    Coroutine m_AttackWaitCoroutine;

    const float k_AttackInputDuration = 0.03f;

    void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        m_AttackInputWait = new WaitForSeconds(k_AttackInputDuration);

        if (s_Instance == null)
            s_Instance = this;
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "CameraX")
            return CameraInput.x;

        else if (axisName == "CameraY")
            return CameraInput.y;

        return 0;
    }

    public void OnMove(InputAction.CallbackContext ctx) => m_Movement = ctx.ReadValue<Vector2>(); 
    public void OnLook(InputAction.CallbackContext ctx) => m_Camera = ctx.ReadValue<Vector2>();

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("attack");
            if (m_AttackWaitCoroutine != null)
                StopCoroutine(m_AttackWaitCoroutine);

            m_AttackWaitCoroutine = StartCoroutine(AttackWait());
        }
    }

    //DEBUT TEST
    public void TestInput(InputAction.CallbackContext ctx) {
        if (ctx.started)
        {
            m_Test = true;
        }
        Debug.Log("input");
    } 
    //FIN TEST
    public void OnJump(InputAction.CallbackContext ctx) => m_Jump = ctx.started;
    public void OnAbility1(InputAction.CallbackContext ctx) => m_Ability1 = ctx.started;
    public void OnAbility2(InputAction.CallbackContext ctx) => m_Ability2 = ctx.started;
    public void OnRun(InputAction.CallbackContext ctx) => m_Run = ctx.performed;
    public void OnWalk(InputAction.CallbackContext ctx) { if (ctx.started) m_Walk = !m_Walk; }
    public void OnEquipWeapon(InputAction.CallbackContext ctx)
    {

    }
    IEnumerator AttackWait()
    {
        m_Attack = true;

        yield return m_AttackInputWait;

        m_Attack = false;
    }

    public bool HaveControl()
    {
        return !m_ExternalInputBlocked;
    }

    public void ReleaseControl()
    {
        m_ExternalInputBlocked = true;
    }

    public void GainControl()
    {
        m_ExternalInputBlocked = false;
    }

    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.started && (GameManager.Instance.IsPlaying || GameManager.Instance.IsInInventory))
        {
            OpenCloseInventory();
        }
    }

    public void OpenEquipmentSheet(InputAction.CallbackContext ctx)
    {
        if (ctx.started && (GameManager.Instance.IsPlaying || GameManager.Instance.IsInInventory))
        {
            OpenCloseEquipmentSheet();
        }
    }
    public void OpenQuestWindow(InputAction.CallbackContext ctx)
    {
        if (ctx.started && (GameManager.Instance.IsPlaying || GameManager.Instance.IsInInventory))
        {
            OpenCloseQuestWindow();
        }
    }
    public void NextLineDialogue(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.IsInDialogue && ctx.started && DialogueManager.Instance.timeSinceLastLine > 2f)
        {
            DialogueManager.Instance.ContinueDialogue();
        }
    }
    public void OpenCloseInventory()
    {
        if (m_Inventory.activeSelf && !m_EquipmentSheet.activeSelf && !m_QuestWindow.activeSelf)
            GameManager.Instance.Play();
        else
            GameManager.Instance.Inventory();
        m_Inventory.SetActive(!m_Inventory.activeSelf);
    }

    public void OpenCloseQuestWindow()
    {
        if (m_QuestWindow.activeSelf && !m_EquipmentSheet.activeSelf && !m_Inventory.activeSelf)
            GameManager.Instance.Play();
        else
            GameManager.Instance.Inventory();
        m_Inventory.SetActive(!m_QuestWindow.activeSelf);
    }

    public void OpenCloseEquipmentSheet()
    {
        if (m_EquipmentSheet.activeSelf && !m_Inventory.activeSelf && !m_QuestWindow.activeSelf)
            GameManager.Instance.Play();
        else
            GameManager.Instance.Inventory();
        m_EquipmentSheet.SetActive(!m_EquipmentSheet.activeSelf);
    }



}

