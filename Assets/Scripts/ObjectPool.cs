using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject[] prefab;
    private List<GameObject> pool;
    private bool canGrow;
    int select;

    public ObjectPool(GameObject[] prefab, int size, bool canGrow)
    {
        this.prefab = prefab;
        this.canGrow = canGrow;
        pool = new List<GameObject>();
        

        for (int i = 0; i < size; i++)
        {
            select = Random.Range(0, prefab.Length);
            GameObject temp = GameObject.Instantiate(prefab[select]);
            temp.SetActive(false);
            pool.Add(temp);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        if (canGrow)
        {
            select = Random.Range(0, prefab.Length);
            GameObject temp = GameObject.Instantiate(prefab[select]);
            pool.Add(temp);
            return temp;
        }

        return null;
    }

    public List<GameObject> objects
    {
        get => pool;
    }
}
