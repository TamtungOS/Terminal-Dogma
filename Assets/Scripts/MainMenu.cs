using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartGameClick()
    {
        SceneManager.LoadScene("World1");
    }
}
