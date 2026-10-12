using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector3 offset;

    private void FixedUpdate()
    {
        // Camera follows the player with specified offset position
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
