using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    [SerializeField] GameObject PauseMenuCanvas;
    [SerializeField] GameObject ResumeButton;
    [SerializeField] GameObject QuitGameButton;

void Start()
    {
        if (PauseMenuCanvas != null)
        {
            PauseMenuCanvas.SetActive(false);
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenuCanvas.SetActive(true);
        IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenuCanvas.SetActive(false);
        IsPaused = false;
    }

    private void QuitToMenu()
    {
        Time.timeScale = 1;
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
    
    void OnPause(InputValue value)
    {
        if (Time.timeScale == 0f && !IsPaused) return;
        
        Debug.Log("Game Pause Input Detected!!");
        if (value.isPressed && !IsPaused)
        {
            PauseGame();
        }
        else if (value.isPressed && IsPaused)
        {
            ResumeGame();
        }
    }

    public void OnResumeButtonClick()
    {
        ResumeGame();
    }

    public void OnQuitButtonClick()
    {
        QuitToMenu();
    }
}
