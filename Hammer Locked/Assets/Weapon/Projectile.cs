using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    Vector3 direction; 
    ProjectileDataSO projectileData; 
    float damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(DestroyAfterSeconds(2f));
    }
    public void SetDirection(Vector3 direction) { this.direction = direction; } 
    //public void SetSpeed(float projectileSpeed) { this.projectileSpeed = projectileSpeed; }
    public void SetProjectileData(ProjectileDataSO projectileData) { this.projectileData = projectileData; }
    public void SetDamage(float damage) { this.damage = damage; }

    void FixedUpdate()
    {
        // Debug.Log("Here");
        // Debug.Log("Direction: " + direction);
        // Debug.Log("projectileSpeed: " + projectileSpeed); 
        rb.MovePosition(transform.position + projectileData.projectileSpeed * Time.fixedDeltaTime * direction);
    }

    IEnumerator DestroyAfterSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);  
        Destroy(this.gameObject); 
    }

    bool hasHit = false;
    void OnTriggerEnter(Collider other)
    {
        if(hasHit) return; //make sure only damages once. Without this it double hits

        IDamageable damageable = GetComponentInParent<IDamageable>() ?? GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.TryTakeDamage(damage); 
            // Debug.Log("Enemy recognizes IDameable. Try Damage");
            // Debug.Log("Damage: " + damage);
            Destroy(this.gameObject);
            hasHit = true;
        }
    }
}
