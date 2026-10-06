using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    private Transform platformPosition;
    void Start()
    {
        platform.position = new Vector2 (103, -5.5f);
    }

}
