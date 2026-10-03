using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    public static SimplePool simplePoolInstance;
    public GameObject prefab;
    public int poolSize = 10;
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (simplePoolInstance == null)
        {
            simplePoolInstance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (simplePoolInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);

            obj.transform.SetParent(this.transform);
            
            obj.SetActive(false);
            pool.Add(obj);

            obj.transform.SetParent(this.transform); 
        }
    }

    // Update is called once per frame
public GameObject GetFromPool()
{
    // A standard for loop allows us to repair elements inside the list safely
    for (int i = 0; i < pool.Count; i++)
    {
        // 1. Check if the scene transition destroyed this bullet asset instance
        if (pool[i] == null)
        {
            // Re-instantiate a fresh bullet slot to repair the hole in the list
            GameObject freshObj = Instantiate(prefab);
            freshObj.transform.SetParent(this.transform);
            freshObj.SetActive(false);
            pool[i] = freshObj; 
        }

        GameObject obj = pool[i];

        // 2. Fetch the inactive bullet
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
