using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject prefab;   //object to pool
    public int poolSize = 10;   //number of objects to pre-instantiate

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        //Create pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        //Try to find an inactive object
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].SetActive(false);
        }

        var obj = pool[0];
        obj.SetActive(true);
        return obj;
    }
}
