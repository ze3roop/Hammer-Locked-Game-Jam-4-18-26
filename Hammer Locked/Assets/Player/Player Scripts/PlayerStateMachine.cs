using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour, IDamageable
{
    public static PlayerStateMachine Instance {get; private set;}

    private PlayerStateManager _mainStateType; 
    private PlayerStateManager _currentState;
    public PlayerStateManager CurrentState { get { return _currentState; } }
    private PlayerStateManager _nextState; 

    public PlayerAttackState PlayerAttackState; 
    public PlayerIdleState PlayerIdleState;
    public PlayerTakeDamageState PlayerTakeDamageState; 
    

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldownMax;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    public RaycastHit slopeHit;
    public bool exitingSlope;

    #region INPUT Parameters
    public Vector2 movement; 
    public bool jumpStarted; 
    public bool sprintStarted; 
    public bool crouchStarted;
    public bool attackStarted;
    public bool attackPerforming; 
    #endregion

    public Transform orientation;

   [HideInInspector] public Rigidbody rb;

    public LayerMask enemyLayerMask; 

    public Animator animator; 

    public void TryTakeDamage(float damage)
    {
        // take damage
        GameEventsManager.Instance.healthEvents.HealthLost(damage); 
        SetNextState(PlayerTakeDamageState); 
    }
    
    public event Action<GameObject> OnApplyDamage;
    public void ApplyDamage(GameObject other)
    {
        OnApplyDamage?.Invoke(other); 
    }

    public event Action OnEnableHitbox; 
    public void EnableAttackHitbox()
    {
        OnEnableHitbox?.Invoke();
    }

    public event Action OnDisableHitbox;
    public void DisableAttackHitbox()
    {
        OnDisableHitbox?.Invoke();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Singleton
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; 

        animator = GetComponentInChildren<Animator>(); 

        PlayerIdleState = new PlayerIdleState();
        PlayerAttackState = new PlayerAttackState();
        PlayerTakeDamageState = new PlayerTakeDamageState();


        _mainStateType ??= PlayerIdleState;
        SetNextStateToMain();

        enemyLayerMask = LayerMask.GetMask("Enemy");
    }
    #region StateMachine Methods
    private void SetState(PlayerStateManager newState) {
        _nextState = null;
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter(this);
    }
    public void SetNextState(PlayerStateManager newState) {
        if (newState != null) {
            _nextState = newState;
        }
    }
    public void SetNextStateToMain() {
        _nextState = _mainStateType;
    }
    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
         if (_nextState != null) {
            SetState(_nextState);
        }
        _currentState?.OnUpdate();
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private void FixedUpdate()
    {
        _currentState?.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        _currentState?.OnLateUpdate();
    }
    #region INPUT
    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.OnMovementPressed += OnMovementPressed; 
        GameEventsManager.Instance.inputEvents.OnMovementCanceled += OnMovementCanceled; 
        GameEventsManager.Instance.inputEvents.OnJumpStarted += OnJumpStarted; 
        GameEventsManager.Instance.inputEvents.OnJumpCanceled += OnJumpCanceled;
        GameEventsManager.Instance.inputEvents.OnSprintStarted += OnSprintStarted;
        GameEventsManager.Instance.inputEvents.OnSprintCanceled += OnSprintCanceled;
        GameEventsManager.Instance.inputEvents.OnCrouchStarted += OnCrouchStarted;
        GameEventsManager.Instance.inputEvents.OnCrouchCanceled += OnCrouchCanceled; 
        GameEventsManager.Instance.inputEvents.OnFireStarted += OnFireStarted;
        GameEventsManager.Instance.inputEvents.OnFirePerforming += OnFirePerforming;
        GameEventsManager.Instance.inputEvents.OnFireCanceled += OnFireCanceled;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.inputEvents.OnMovementPressed -= OnMovementPressed; 
        GameEventsManager.Instance.inputEvents.OnMovementCanceled -= OnMovementCanceled; 
         GameEventsManager.Instance.inputEvents.OnJumpStarted -= OnJumpStarted; 
        GameEventsManager.Instance.inputEvents.OnJumpCanceled -= OnJumpCanceled;
        GameEventsManager.Instance.inputEvents.OnSprintStarted -= OnSprintStarted;
        GameEventsManager.Instance.inputEvents.OnSprintCanceled -= OnSprintCanceled;
        GameEventsManager.Instance.inputEvents.OnCrouchStarted -= OnCrouchStarted;
        GameEventsManager.Instance.inputEvents.OnCrouchCanceled -= OnCrouchCanceled; 
        GameEventsManager.Instance.inputEvents.OnFireStarted -= OnFireStarted;
        GameEventsManager.Instance.inputEvents.OnFirePerforming -= OnFirePerforming;
        GameEventsManager.Instance.inputEvents.OnFireCanceled -= OnFireCanceled; 
    }
    private void OnMovementPressed(UnityEngine.Vector2 movement)
    {
        this.movement = movement; 
    }
    private void OnMovementCanceled(UnityEngine.Vector2 movement)
    {
        this.movement = movement; 
    }
    private void OnJumpStarted()
    {
        jumpStarted = true;
    }
    private void OnJumpCanceled()
    {
        jumpStarted = false;
    }
    private void OnSprintStarted()
    {
        sprintStarted = true;
    }
    private void OnSprintCanceled()
    {
        sprintStarted = false;
    }
    private void OnCrouchStarted()
    {
        crouchStarted = true;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }
    private void OnCrouchCanceled()
    {
        crouchStarted = false;
    }
    private void OnFireStarted()
    {
        attackStarted = true;
    }
    private void OnFirePerforming()
    {
        attackPerforming = true;
    }
    private void OnFireCanceled()
    {
        attackStarted = false;
        attackPerforming = false;
    }
    #endregion
} 