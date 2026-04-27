using UnityEngine;
using UnityEngine.AI;

public class Wall : MonoBehaviour, IDamageable
{
    public  SoundData smashWall;
    public SoundData wallFalls; 
    public float currentHealth = 200f;

    public Transform destroyedWall_prefab; 
    public NavMeshObstacle obstacle; 

    public void TryTakeDamage(float damage)
    {
        GameEventsManager.Instance.audioEvents.PlaySound(smashWall, this.transform.position); 
        currentHealth -= damage; 
        if(currentHealth <= 0)
        {
            GameEventsManager.Instance.audioEvents.PlaySound(wallFalls, this.transform.position); 
            InitiateDestruction(); 
        }
    }
    public float explosionForce;
    public float explositionRadius;
    public void InitiateDestruction()
    {
        GameEventsManager.Instance.healthEvents.HealthGained(5f);

        var parent = transform.parent;

        Transform wallBroken = Instantiate(
            destroyedWall_prefab,
            parent.position,
            parent.rotation,
            parent
        );

        wallBroken.localScale = parent.localScale;
        foreach (Transform child in wallBroken)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, PlayerStateMachine.Instance.transform.position, explositionRadius); 
            }
        }
        if (obstacle != null)
            obstacle.enabled = false;
        
        Destroy(gameObject); 
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("HIT WALL");
    //         GameEventsManager.Instance.audioEvents.PlaySound(smashWall, this.transform.position); 
    //     }
    // }
}
