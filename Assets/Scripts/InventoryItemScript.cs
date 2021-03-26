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
            FoodManagerScript.Instance.AddItem(item.GetComponent<FoodItemScript>());
            count--;
            Prefs.SetInventoryBurger(count);
            return item;
        }
        return null;
    }

    private void OnEnable()
    {
        if (foodType == FoodItemScript.FoodItemType.BURGER) {
            UpdateInventory(Prefs.GetInventoryBurger());
        }
        Messenger.AddListener<int>(GameEvent.CHANGED_INVENTORY_BURGER, UpdateInventory);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<int>(GameEvent.CHANGED_INVENTORY_BURGER, UpdateInventory);
    }

    public void UpdateInventory(int value)
    {
        print(value);
        if (foodType == FoodItemScript.FoodItemType.BURGER)
        {
            count = value;
        }
    }
}
