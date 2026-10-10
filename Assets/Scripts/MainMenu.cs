using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartGameClick()
    {
        LevelGoal.ResetTimer();
        SceneManager.LoadScene("World1");
    }
    
    public void OnQuitGameClick()
    {
        Application.Quit();
    }
}
