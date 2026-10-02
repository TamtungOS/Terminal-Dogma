using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 20;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    } 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Health" + health);
    }
}
