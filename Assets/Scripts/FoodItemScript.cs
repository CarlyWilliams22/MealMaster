using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // cook durations in seconds
    public static readonly Dictionary<FoodItemType, float> cookDurations = new Dictionary<FoodItemType, float>
    {
        { FoodItemType.DRINK, 3.0f },
        { FoodItemType.BURGER, 10.0f }
    };

    public FoodItemType type;
    public float cookDuration
    {
        get => cookDurations[type];
    }

    public float retailPrice
    {
        get => retailPrices[type];
    }

    public float wholesalePrice
    {
        get => wholesalePrices[type];
    }
}
