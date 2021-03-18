using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemScript : MonoBehaviour
{
    public enum FoodItemType {
        DRINK, BURGER
    }

    public float costWholesale; // restocking price
    public float costRetail;    // price customer pays
    public FoodItemType type;
    public float cookDuration;
}
