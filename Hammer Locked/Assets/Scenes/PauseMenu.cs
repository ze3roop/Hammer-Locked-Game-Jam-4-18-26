using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool pauseStarted;
    public bool isPaused;
    public void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.OnPauseStarted += OnPauseStarted;
    }
    public void OnDisable()
    {
        GameEventsManager.Instance.inputEvents.OnPauseStarted -= OnPauseStarted;
    }

    private void OnPauseStarted()
    {
        Debug.Log("PAUSE STARTED");
        //  pauseStarted = true;
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();   
        }
    }

    public void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        GameEventsManager.Instance.inputEvents.IsPaused(false); 
    }
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Debug.Log("PAUSE GAME");
        pauseMenu.SetActive(true); 
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
