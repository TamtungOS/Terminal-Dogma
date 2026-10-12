using UnityEngine;

public class MainMenuBackgroundScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;       // units per second
    [SerializeField] private float patternWidth = 71.12f;  // width of one normal copy plus one flipped copy

    private float startX;

    private void Start()
    {
        startX = transform.position.x;
    }

    private void Update()
    {
        // Slide the background to the left
        transform.position = transform.position + Vector3.left * scrollSpeed * Time.deltaTime;

        // After sliding one full pattern, jump back to the start.
        // The sky looks identical at both positions, so the jump can't be seen.
        if (transform.position.x <= startX - patternWidth)
        {
            transform.position = transform.position + Vector3.right * patternWidth;
        }
    }
}