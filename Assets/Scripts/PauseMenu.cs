using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    [SerializeField] private GameObject pauseMenuCanvas;

    private void Start()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
        IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuCanvas.SetActive(false);
        IsPaused = false;
    }

    private void QuitToMenu()
    {
        Time.timeScale = 1;
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnPause(InputValue value)
    {
        if (Time.timeScale == 0f && !IsPaused) return;

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
