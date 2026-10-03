using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class World1 : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private GameObject player;
    [SerializeField] private CinemachineCamera worldCinemachineCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            player.transform.position = spawnPoint.position;

            if (worldCinemachineCamera != null)
            {
                worldCinemachineCamera.Follow = player.transform;
                worldCinemachineCamera.LookAt = player.transform;
                
                // 4. Force the camera to snap its position instantly 
                // so it doesn't slowly slide from its old location to the sky
                worldCinemachineCamera.OnTargetObjectWarped(player.transform, player.transform.position - worldCinemachineCamera.transform.position);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
