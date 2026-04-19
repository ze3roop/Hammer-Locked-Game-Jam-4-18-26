
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform cameraOrientation; 

    float xRotation;
    float yRotation;

    Vector2 mouseXY; 

    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.OnLook += OnLook; 
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.inputEvents.OnLook -= OnLook;
    }

    private void OnLook(UnityEngine.Vector2 look)
    {
        mouseXY = look;  
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 delta = mouseXY;
        // Debug.Log("delta: " + delta); 
        // mouseXY = Vector2.zero; // <- THIS FIXES DRIFT

        float mouseX = delta.x * Time.deltaTime * sensX;
        float mouseY = delta.y * Time.deltaTime * sensY;
        
        yRotation += mouseX ;
        xRotation -= mouseY;
        // Debug.Log("xRotation: " + xRotation); 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraOrientation.rotation = Quaternion.Euler(0, yRotation, 0); 

    }

}