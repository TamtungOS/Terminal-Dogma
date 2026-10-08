using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
    }
}
