using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemScript : MonoBehaviour
{
    public GameObject spawnerPrefab;
    public int count;

    public bool CanInstantiateItem()
    {
        return count > 0;
    }

    public GameObject InstantiateItem()
    {
        if (CanInstantiateItem())
        {
            GameObject item = Instantiate(spawnerPrefab, transform.position, Quaternion.identity);
            FoodManagerScript.Instance.AddItem(item.GetComponent<FoodItemScript>());
            count--;
            return item;
        }
        return null;
    }
}
