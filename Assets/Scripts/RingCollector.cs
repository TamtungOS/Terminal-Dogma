using UnityEngine;

public class RingCollector : MonoBehaviour
{
    private int totalRings = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Total Rings:" + totalRings);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
     if (collision.CompareTag("Ring"))
        {
            totalRings++;
            Debug.Log("Total Rings: " + totalRings);
        }   
    }
}
