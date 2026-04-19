
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    // public Vector3 offset; 

    private void Start()
    {
        
        // transform.rotation = Quaternion.Euler(0, 90f, 0); 
    }

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
