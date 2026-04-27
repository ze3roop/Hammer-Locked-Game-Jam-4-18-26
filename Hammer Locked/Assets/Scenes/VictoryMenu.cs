using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public static VictoryMenu Instance { get; private set; }
    
    public GameObject victoryMenu;
    public GameObject healthbar;
    public GameObject healthbar2;
    public bool pauseStarted;
    public bool isPaused;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager");
        }
        Instance = this;
    }

    public void Start()
    {
        victoryMenu.SetActive(false);
    }
    public void VictoryStart()
    {
        healthbar.SetActive(false);
        healthbar2.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Debug.Log("PAUSE GAME");
        victoryMenu.SetActive(true); 
        Time.timeScale = 0f;
        isPaused = true;
        GameEventsManager.Instance.inputEvents.IsPaused(true); 
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
