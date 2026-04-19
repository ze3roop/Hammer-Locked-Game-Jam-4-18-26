using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    [HideInInspector]
    public InputEvents inputEvents;
    // public HealthEvents healthEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager");
        }
        Instance = this;

        inputEvents = new InputEvents(); 
        // healthEvents = new(); 
    }
}
