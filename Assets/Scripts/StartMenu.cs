using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // For Unity 6 Input
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput; // Drag your player's PlayerInput component here
    [SerializeField] private Button startGameButton; 
    [SerializeField] private TextMeshProUGUI startGameButtonText;
    [SerializeField] private GameObject physicalPlatform; // Drag the StartButton_Physics GameObject here
    private AsyncOperation backgroundLoadOperation;
    [SerializeField] private string firstWorldSceneName = "World1";

    void Start()
    {
        startGameButtonText.text = "START GAME"; 
        StartCoroutine(PreloadWorldInBackground());

        // 1. Lock the player down immediately when the game boots up
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

        // 2. Listen for the UI Button click event
        startGameButton.onClick.AddListener(StartGameSequence);
    }

    void StartGameSequence()
    {
        // 3. Give control back to the player
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }

        // 4. Disable the UI button visually so they can't click it again
        startGameButton.gameObject.SetActive(true);
    }
        private IEnumerator PreloadWorldInBackground()
    {
        yield return null;
        // Begin loading in the background
        backgroundLoadOperation = SceneManager.LoadSceneAsync(firstWorldSceneName);

        // CRITICAL: Tell Unity NOT to open the scene yet when it finishes loading.
        // Just hold it silently in RAM.
        backgroundLoadOperation.allowSceneActivation = false;

        // Keep the coroutine alive while it buffers
        while (backgroundLoadOperation.progress < 0.9f)
        {
            yield return null;
        }
        
        Debug.Log("World 1 is fully buffered in RAM and ready to instant-swap!");
    }

    // A public function that our Trigger Zone can call later to instantly flip the switch
    public void ActivatePreloadedScene()
    {
        if (backgroundLoadOperation != null)
        {
            backgroundLoadOperation.allowSceneActivation = true;
        }
    }
}
