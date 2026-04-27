using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement), typeof(EnemyCombat), typeof(EnemySenses))]
public class EnemyBrain : MonoBehaviour, IDamageable
{
    // EnemyData holds all static parameters of the enemy, such as sightRange, attackRange, wanderRadius
    [SerializeField] EnemyDataSO data;
    // [SerializeField] Transform player;
    // [SerializeField] Transform muzzle;
    [HideInInspector] public Animator animator; 
    public Rigidbody mainRigidbody;
    public Collider mainCollider; 

    // These 3 components are attached to every Enemy gameobject. Each script distinctly controls an aspect of the enemy. Keeps things separated and modulated. 
    EnemyMovement _move;
    EnemyCombat _combat;
    EnemySenses _senses;

    // StateMachine used to enter, exit, and update whatever the current state is
    StateMachine _sm = new StateMachine();

    // Define the Enemy states
    private IEnemyState _idle, _patrol, _chase, _getInRange, _attack, _search, _reposition, _melee, _inCover, _default, _takeDamage;

    
    // Public accessors for states
    public IEnemyState Patrol => _patrol;
    public IEnemyState Chase => _chase;
    public IEnemyState GetInRange => _getInRange;
    public IEnemyState Attack => _attack;
    public IEnemyState Search => _search;
    public IEnemyState Reposition => _reposition;
    public IEnemyState Melee => _melee;
    public IEnemyState InCover => _inCover; 
    public IEnemyState Default => _default;
    public IEnemyState TakeDamageState => _takeDamage; 

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;

    public float maxHealth; 
    public float currentHealth; 

    #region SOUNDS
    public SoundData bodyhitSounds;
    public SoundData groanSounds;
    public SoundData deathSounds;

    public SoundData attackSounds; 
    #endregion
    public ParticleSystem hitEffect;

    //To have our source destination for our patrol radius
    public UnityEngine.Vector3 startingPosition;

    void Awake()
    {
        startingPosition = this.transform.position;

        _move = GetComponent<EnemyMovement>();
        _combat = GetComponent<EnemyCombat>();
        _senses = GetComponent<EnemySenses>();

        animator = GetComponent<Animator>();
        mainRigidbody = GetComponent<Rigidbody>();
        mainCollider = GetComponent<Collider>();

        // Get EVERYTHING in children
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    [SerializeField] private float lateralForceMin = 400f;
    [SerializeField] private float lateralForceMax = 800f;

    [SerializeField] private float upwardForceMin = 150f;
    [SerializeField] private float upwardForceMax = 300f;

    [SerializeField] private string ragdollLayerName = "EnemyRagdoll";

    void SetLayerRecursive(Transform root, int layer)
    {
        root.gameObject.layer = layer;

        foreach (Transform child in root)
        {
            SetLayerRecursive(child, layer);
        }
    }

    public void EnableRagdoll()
    {
        int ragdollLayer = LayerMask.NameToLayer(ragdollLayerName);
        SetLayerRecursive(transform, ragdollLayer);

        float lateralForce = UnityEngine.Random.Range(lateralForceMin, lateralForceMax);
        float upwardForce = UnityEngine.Random.Range(upwardForceMin, upwardForceMax);

        UnityEngine.Vector3 forceDirection = transform.position - PlayerStateMachine.Instance.transform.position;
        forceDirection.y = 0f;
        forceDirection.Normalize();

        // Disable AI / movement
        _move.Agent.enabled = false;

        // Disable main collider so it doesn't fight ragdoll
        mainCollider.enabled = false;

        // Disable main rigidbody (so it doesn't interfere)
        mainRigidbody.isKinematic = true;
        mainRigidbody.useGravity = false;

        // Turn OFF animator (so physics takes over bones)
        animator.enabled = false;

        // Enable ragdoll physics
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb == mainRigidbody) continue;

            rb.isKinematic = false;
            rb.useGravity = true;

            // Apply explosive force
            rb.AddForce(forceDirection * lateralForce, ForceMode.Impulse);
            rb.AddForce(UnityEngine.Vector3.up * upwardForce, ForceMode.Impulse);
        }

        // Enable ragdoll colliders
        foreach (Collider col in ragdollColliders)
        {
            if (col == mainCollider) continue;

            col.enabled = true;
        }

        //Disable attackhitbox after all of the above runs.
        _combat.DisableAttackHitbox();
    }

    void DisableRagdoll()
    {
        _move.Agent.enabled = true;
        mainCollider.enabled = true;

        mainRigidbody.isKinematic = true;
        mainRigidbody.useGravity = false;

        // Turn OFF physics on all ragdoll parts
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if(rb == mainRigidbody) continue;
            rb.isKinematic = true;
        }

        // (Optional but recommended) disable ragdoll colliders
        foreach (Collider col in ragdollColliders)
        {
            if(col == mainCollider) continue;
            col.enabled = false;
        }

        // Turn animation back ON
        animator.enabled = true;
    }

    void Start()
    {
        currentHealth = maxHealth;
        //Define all states that the enemy and what its specific archetype uses
        BuildStates();
        //_sm.ChangeState(_patrol); // start here for demo
    }

    void Update()
    {
        _senses.OnUpdate(PlayerStateMachine.Instance.transform, data.sightRange);
        _sm.OnUpdate();

        if (IsInCover)
        {
            inCover_duration += Time.deltaTime; 
        }
        animator.SetFloat("MovementSpeed", _move.MovementSpeed());
    }

    void BuildStates()
    {
        // general states shared by all enemies
        _patrol    = new PatrolState(this, data);
        _chase     = new ChaseState(this, data);
        // _getInRange = new GetInRangeState(this, data);
        _attack    = new AttackState(this, data);
        _search    = new SearchState(this, data);

        // optional states depending on the archetype 
        _reposition = new RepositionState(this, data);
        _melee      = new MeleeState(this, data);
        
        _inCover = new InCoverState(this, data); 
        _default = new DefaultState(this, data); 
        _takeDamage = new TakeDamageState(this, data); 
        _idle = new IdleState(this,data);

        // Decide starting state and allowed transitions
        switch (data.archetype)
        {
            case EnemyArchetype.Grunt:
                // _reposition = null;
                // _melee = null;

                _sm.ChangeState(_patrol);
                break;

            // case EnemyArchetype.Sniper:
            //     // _melee = null;

            //     // // If sniper uses vantage points, start by repositioning
            //     // if (data.useVantagePoints)
            //     //     _sm.ChangeState(_reposition);
            //     // else
            //     //     _sm.ChangeState(_patrol);
            //     // break;

            // case EnemyArchetype.Rusher:
            //     _getInRange = null;

            //     _sm.ChangeState(_patrol);
            //     break;
        }
    }
    public event Action OnTakeDamage;
    public void TryTakeDamage(float damage)
    {
        
        _sm.ChangeState(_takeDamage); 
        // OnTakeDamage?.Invoke(); 
        GameEventsManager.Instance.audioEvents.PlaySound(bodyhitSounds, this.transform.position);
        currentHealth -= damage; 
        if(currentHealth <= 0)
        {
            Death();
        }
        else
        {
            GameEventsManager.Instance.audioEvents.PlaySound(groanSounds, this.transform.position); 
        }
    }

    public void Death()
    {
        GameEventsManager.Instance.audioEvents.PlaySoundFollowObject(deathSounds, gameObject);
        EnableRagdoll(); 
        StartCoroutine(DestroyTimer());
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject); 
    }
    
    public bool HasTargetLOS() => _senses.HasLOS;
    public bool RecentlySawTarget() => _senses.RecentlySawTarget(data.loseSightAfterSeconds);
    public Transform Target => PlayerStateMachine.Instance.transform;
    public EnemyMovement Move => _move;
    public EnemyCombat Combat => _combat;
     // public Transform Muzzle => muzzle;
    public bool IsInCover = false; 
    public float inCover_duration; 

    public CoverPointData currentCoverPoint; 

    public float DistToTarget()
    {
        return UnityEngine.Vector3.Distance(transform.position, PlayerStateMachine.Instance.transform.position);
    }

    public void SetState(IEnemyState s) => _sm.ChangeState(s);
}
