using UnityEngine;

public class DeathFall : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.DeathFall();
            Destroy(gameObject);
        }
    }
}
