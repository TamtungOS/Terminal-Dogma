using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    private Transform platformPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platform.position = new Vector2 (103, -5.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
