using UnityEngine;

public class Ring : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.AddRing();

            gameObject.SetActive(false);
        }
    }
}
