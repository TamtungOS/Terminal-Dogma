using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float elapsedTime = 0f;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        timerText.text = "Time: " + elapsedTime.ToString("F1");
    }

    private void OEnter2D(Collider2D collision)
    {
     PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
     int rings = playerHealth.GetRings();

     int bestRings = RingsAndTimePlayerPrefs.GetRings();
     if (rings > bestRings)
        {
            RingsAndTimePlayerPrefs.SetRings(rings);
        }

        float bestTime = RingsAndTimePlayerPrefs.GetTime();
        if (bestTime == 0f || elapsedTime < bestTime)
        {
         RingsAndTimePlayerPrefs.SetTime(elapsedTime);   
        }

        RingsAndTimePlayerPrefs.SavePlayerPrefs();
        SceneManager.LoadScene("MainMenu");
    }
}