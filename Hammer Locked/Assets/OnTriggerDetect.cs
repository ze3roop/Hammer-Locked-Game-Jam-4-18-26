using UnityEngine;

public class OnTriggerDetect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<AttackCollider>().OnTriggerEnter(other); 
    }
}
