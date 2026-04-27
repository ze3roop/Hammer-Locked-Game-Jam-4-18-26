using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float min = 3f;
    public float max = 8f;
    public float duration; 
    void Start()
    {
        duration = Time.time + Random.Range(min, max); 
    }
    void Update()
    {
        if(Time.time > duration)
        {
            Destroy(gameObject); 
        }
    }
}
