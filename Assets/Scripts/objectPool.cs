using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPool
{
    GameObject prototype;
    List<GameObject> pool;
    public bool canGrow;

    public objectPool(GameObject prototype, bool resizeable, int count)
    {
        this.prototype = prototype;
        this.canGrow = resizeable;
        pool = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject temp = Object.Instantiate(prototype);
            temp.SetActive(false);
            pool.Add(temp);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        if (canGrow)
        {
            GameObject temp = Object.Instantiate(prototype);
            pool.Add(temp);
            return temp;
        }
        return null;
    }
}