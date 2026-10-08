using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public SimplePool pool;
    public Transform firePoint;

    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
        Fire();
        }
    }

    private void Fire()
    {
        if (PauseMenu.IsPaused) return;

        GameObject obj = pool.GetFromPool();

        if (obj != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

            obj.transform.position = spawnPos;
            obj.transform.rotation = transform.rotation;

            float direction = transform.lossyScale.x > 0 ? 1f: -1f;

            KiBlast blast = obj.GetComponent<KiBlast>();

            if (blast != null)
            {
                blast.SetDirection(direction);
                blast.Initialize(pool);
                blast.ClearTrail();
            }
        }
    }
}
