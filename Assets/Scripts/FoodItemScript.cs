using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodItemScript : MonoBehaviour
{
    public enum FoodItemType {
        DRINK, BURGER
    }

    // restocking prices
    public static readonly Dictionary<FoodItemType, float> wholesalePrices = new Dictionary<FoodItemType, float>
    {
        { FoodItemType.DRINK, 0.25f },
        { FoodItemType.BURGER, 0.75f }
    };

    // what the customer pays
    public static readonly Dictionary<FoodItemType, float> retailPrices = new Dictionary<FoodItemType, float>
    {
        { FoodItemType.DRINK, 1.00f },
        { FoodItemType.BURGER, 2.50f }
    };

    public FoodItemType type;
    public float timeCooked; // time cooked so far
    public float cookDuration; // total time it takes to cook
    public float burnDuration; // time in seconds after the food is fully cooked that it takes to burn it
    public List<string> cookerTags; // list of tags of gameobjects than can cook this food item when within their trigger collider

    private bool isCooking; // is being cooked

    private void Update()
    {
        if (isCooking)
        {
            timeCooked += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            isCooking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            isCooking = false;
        }
    }

    public float retailPrice
    {
        get => retailPrices[type];
    }

    public float wholesalePrice
    {
        get => wholesalePrices[type];
    }

    public bool isBurnt
    {
        get => timeCooked >= burnDuration;
    }

    public bool isCooked
    {
        get => timeCooked > cookDuration && !isBurnt;
    }
}
