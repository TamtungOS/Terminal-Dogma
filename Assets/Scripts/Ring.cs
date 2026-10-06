using UnityEngine;

public class Ring : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Debug.Log(gameObject.name);
            Destroy(gameObject);
        }
    }

}
