using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldTriggerZone : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] private string firstWorldSceneName = "World1";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(firstWorldSceneName);

            DontDestroyOnLoad(other.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
