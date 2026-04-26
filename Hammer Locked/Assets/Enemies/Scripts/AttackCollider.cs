using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool isPlayer; 
    private void OnEnable()
    {
        if (!isPlayer)
        {
            GetComponentInParent<EnemyCombat>().OnEnableHitbox += OnEnableHitbox;
            GetComponentInParent<EnemyCombat>().OnDisableHitbox += OnDisableHitbox;
        }
        else
        {
            PlayerStateMachine.Instance.OnEnableHitbox += OnEnableHitbox; 
            PlayerStateMachine.Instance.OnDisableHitbox += OnDisableHitbox; 
        }
        
    }
    private void OnDisable()
    {
        if (!isPlayer)
        {
            GetComponentInParent<EnemyCombat>().OnEnableHitbox -= OnEnableHitbox;
            GetComponentInParent<EnemyCombat>().OnDisableHitbox -= OnDisableHitbox;     
        }
        else
        {
            PlayerStateMachine.Instance.OnEnableHitbox -= OnEnableHitbox; 
            PlayerStateMachine.Instance.OnDisableHitbox -= OnDisableHitbox;    
        }
    }

    public Collider hitbox; 
    private void Start()
    {
        hitbox.enabled = false;
        // hitbox = GetComponent<BoxCollider>(); 
    }

    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();
    public void OnTriggerEnter(Collider other)
    {
        if(!hitbox.enabled) return; 

        if(hitObjects.Contains(other.gameObject)) return;

        if (!isPlayer) //&& other.GetComponentInParent<PlayerStateMachine>() == null )
        {
            if (other.CompareTag("Player"))
            {
                GetComponentInParent<EnemyCombat>().MeleeAttackHitPlayer(); 
                hitObjects.Add(other.gameObject);
            }
        }
        else
        {
            PlayerStateMachine.Instance.ApplyDamage(other.gameObject); 
            hitObjects.Add(other.gameObject);
        
        }
        
    }

    public void OnEnableHitbox()
    {
        hitObjects.Clear();
        hitbox.enabled = true;
        
    }
    public void OnDisableHitbox()
    {
        hitbox.enabled = false;
    }
}
