using UnityEngine;
using UnityEngine.InputSystem;

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

    private void QuitGame()
    {
        Application.Quit();
    }
    
    void OnPause(InputValue value)
    {
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
        QuitGame();
    }
}
