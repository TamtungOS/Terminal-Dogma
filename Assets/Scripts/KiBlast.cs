using UnityEngine;
using UnityEngine.EventSystems;

public class KiBlast : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 2f;
    private float moveDirection = 1f;

    private SimplePool originPool;
    [SerializeField] public float damage = 5f;

    public void Initialize(SimplePool poolReference)
    {
        originPool = poolReference;
    }

    public void SetDirection(float dir)
    {
        moveDirection = dir;
    }
    public void ClearTrail()
    {
        GetComponent<TrailRenderer>().Clear();
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    public void Update()
    {
        transform.Translate(Vector3.right * moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.tag;
        //Debug.Log(tagName);

        if (collision.CompareTag("Player")) return;

        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
            enemyHealth.TakeDamage(damage);    
            }
            
        }

        ReturnToPool();
    }
    private void ReturnToPool()
    {
        if (originPool != null)
        {
            originPool.ReturnToPool(gameObject);
        }
        else
        {
        gameObject.SetActive(false);
        }
    }
}
