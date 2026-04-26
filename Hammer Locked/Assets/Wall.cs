using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public  SoundData smashWall;
    public SoundData wallFalls; 
    public float currentHealth = 200f;

    public void TryTakeDamage(float damage)
    {
        GameEventsManager.Instance.audioEvents.PlaySound(smashWall, this.transform.position); 
        currentHealth -= damage; 
        if(currentHealth <= 0)
        {
            GameEventsManager.Instance.audioEvents.PlaySound(wallFalls, this.transform.position); 
            Destroy(gameObject);
        }
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
