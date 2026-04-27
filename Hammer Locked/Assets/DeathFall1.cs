using UnityEngine;

public class DeathFall1 : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.Death();
            Destroy(gameObject);
        }
    }
}
