using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    // 1 = locked to the camera (no scrolling), 0 = scrolls as fast as the ground
    [SerializeField] private float parallaxAmount = 0.9f;

    private float backgroundStartX;
    private float cameraStartX;

    private void Start()
    {
        // Remember where the background and camera began
        backgroundStartX = transform.position.x;
        cameraStartX = cameraTransform.position.x;
    }

    private void LateUpdate()
    {
        // How far the camera has moved since the level started
        float cameraDistance = cameraTransform.position.x - cameraStartX;

        // Move the background only part of that distance
        float newX = backgroundStartX + cameraDistance * parallaxAmount;

        // Follow the camera's height fully so the sky stays centered vertically
        transform.position = new Vector3(newX, cameraTransform.position.y, transform.position.z);
    }
}