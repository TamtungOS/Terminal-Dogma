using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text bestRingsText;

    private void Start()
    {
        float bestTime = RingsAndTimePlayerPrefs.GetTime();
        int bestRings = RingsAndTimePlayerPrefs.GetRings();

        if (bestTime == 0f)
        {
            bestTimeText.text = "Best Time: --";
        }
        else
        {
            bestTimeText.text = "Best Time: " + bestTime.ToString("F1");
        }
        bestRingsText.text = "Most Rings: " + bestRings;
    }
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
