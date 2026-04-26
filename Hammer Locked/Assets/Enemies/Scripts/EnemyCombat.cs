using System;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>(); 
    }

    float _nextFireTime;

    public bool TryShoot(float cooldown, Transform muzzle, Transform target)
    {
        // if (Time.time < _nextFireTime) return false;

        // // Replace with your hitscan/projectile logic
        // Debug.DrawLine(muzzle.position, target.position, Color.red, 0.05f);
        weapon.TryFire(target); 

        //  _nextFireTime = Time.time + cooldown;
        return true;
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

    public void MeleeAttackHitPlayer()
    {
        PlayerStateMachine.Instance.TryTakeDamage(10f); 
    }
}
