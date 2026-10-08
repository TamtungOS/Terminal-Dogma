using UnityEngine;

public class Ring : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.AddRing();

            gameObject.SetActive(false);
        }
    }
}
