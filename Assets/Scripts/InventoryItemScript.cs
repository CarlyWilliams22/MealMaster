using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class InventoryItemScript : MonoBehaviour
{
    public GameObject spawnerPrefab;
    public int count;
    public FoodItemScript.FoodItemType foodType;

    public bool CanInstantiateItem()
    {
        return count > 0;
    }

    public GameObject InstantiateItem()
    {
        if (CanInstantiateItem())
        {
            GameObject item = Instantiate(spawnerPrefab, transform.position, Quaternion.identity);
            switch (foodType)
            {
                case FoodItemScript.FoodItemType.BURGER:
                case FoodItemScript.FoodItemType.DRINK:
                    FoodManagerScript.Instance.AddItem(item.GetComponent<FoodItemScript>());
                    break;
                default:
                    break;
            }
            count--;
            if (foodType == FoodItemScript.FoodItemType.BURGER)
            {
                Prefs.SetInventoryBurger(count);
            }
            return item;
        }
        return null;
    }

    private void OnEnable()
    {
        if (foodType == FoodItemScript.FoodItemType.BURGER)
        {
            UpdateInventory(Prefs.GetInventoryBurger());
        }
        if (foodType == FoodItemScript.FoodItemType.BUN)
        {
            UpdateInventory(Prefs.GetInventoryBun());
        }
        if (foodType == FoodItemScript.FoodItemType.DRINK)
        {
            UpdateInventory(Prefs.GetInventoryCup());
        }
    }

    public void UpdateInventory(int value)
    {
        count = value;
    }
}
