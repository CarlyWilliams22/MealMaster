using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    public FoodItemScript.FoodItemType order;

    private void OnEnable()
    {
        // pick a random item to order
        System.Array foodItems = FoodItemScript.FoodItemType.GetValues(typeof(FoodItemScript.FoodItemType));
        order = (FoodItemScript.FoodItemType)foodItems.GetValue(Random.Range(0, foodItems.Length));

        Messenger.Broadcast(GameEvent.CUSTOMER_CHANGE_ACTIVE, this, true);
    }

    private void OnDisable()
    {
        Messenger.Broadcast(GameEvent.CUSTOMER_CHANGE_ACTIVE, this, false);
    }
}
