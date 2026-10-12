using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [Header("Results Screen")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TMP_Text resultsTimeText;
    [SerializeField] private TMP_Text resultsRingsText;
    [SerializeField] private TMP_Text resultsGradeText;

    [Header("Grading")]
    [SerializeField] private int totalRings = 213;
    [SerializeField] private float fastTime = 45f;   // this time or better earns the full time score
    [SerializeField] private float slowTime = 120f;  // this time or worse earns no time score

    private static float elapsedTime = 0f;
    private bool levelFinished = false;

    private void Start()
    {
        // Hide the results until the player reaches the goal
        resultsPanel.SetActive(false);
    }

    private void Update()
    {
        // Stop counting once the level is finished
        if (!levelFinished)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            timerText.text = "Time: " + elapsedTime.ToString("F1");
        }
    }

    // Called when a new run begins, so the timer starts from zero
    public static void ResetTimer()
    {
        elapsedTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !levelFinished)
        {
            levelFinished = true;

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            int rings = playerHealth.GetRings();

            SaveRecords(rings);
            ShowResults(rings);
        }
    }

    private void SaveRecords(int rings)
    {
        // Save the ring count if it beats the old record
        int bestRings = RingsAndTimePlayerPrefs.GetRings();
        if (rings > bestRings)
        {
            RingsAndTimePlayerPrefs.SetRings(rings);
        }

        // Save the time if it beats the old record (0 means no record yet)
        float bestTime = RingsAndTimePlayerPrefs.GetTime();
        if (bestTime == 0f || elapsedTime < bestTime)
        {
            RingsAndTimePlayerPrefs.SetTime(elapsedTime);
        }

        RingsAndTimePlayerPrefs.SavePlayerPrefs();
    }

    private void ShowResults(int rings)
    {
        resultsTimeText.text = "Time: " + elapsedTime.ToString("F1");
        resultsRingsText.text = "Rings: " + rings;
        resultsGradeText.text = "Grade: " + CalculateGrade(rings);

        resultsPanel.SetActive(true);

        // Freeze the game behind the results screen
        Time.timeScale = 0f;
    }

    private string CalculateGrade(int rings)
    {
        // Rings collected, as a number from 0 to 1
        float ringScore = (float)rings / totalRings;

        // Time, as a number from 0 (slow) to 1 (fast)
        float timeScore = Mathf.InverseLerp(slowTime, fastTime, elapsedTime);

        // The final score is the average of the two
        float finalScore = (ringScore + timeScore) / 2f;

        string grade;

        if (finalScore >= 0.9f)
        {
            grade = "S";
        }
        else if (finalScore >= 0.75f)
        {
            grade = "A";
        }
        else if (finalScore >= 0.6f)
        {
            grade = "B";
        }
        else if (finalScore >= 0.4f)
        {
            grade = "C";
        }
        else
        {
            grade = "D";
        }

        return grade;
    }

    // Hooked up to the Retry button
    public void OnRetryButtonClick()
    {
        Time.timeScale = 1f;
        ResetTimer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hooked up to the Menu button
    public void OnMenuButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}