using UnityEngine;

public class RingCollector : MonoBehaviour
{
    private int totalRings = 0;
    void OnTriggerEnter2D(Collider2D collision)
    {
     if (collision.CompareTag("Ring"))
        {
            totalRings++;
        }   
    }
}
