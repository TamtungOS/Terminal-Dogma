using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Update is called once per frame
    public GameObject GetFromPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.SetParent(null);

                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);

        obj.transform.SetParent(this.transform);
    }
}
