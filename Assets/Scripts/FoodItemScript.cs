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
    public float fireDuration; // time in seconds after food has started to burn until it catches fire
    public List<string> cookerTags; // list of tags of gameobjects than can cook this food item when within their trigger collider
    public GameObject firePrefab;
    public GameObject smokePrefab;

    private bool isCooking; // is being cooked
    private GameObject fire;
    private GameObject smoke;
    private bool hasDroppedOnFloor;
    private bool hasBeenExtinguished; // has touched the fire extinguisher chemicals

    private void Update()
    {
        if (isCooking)
        {
            bool wasBurnt = isBurnt;
            bool wasOnFire = isOnFire;

            timeCooked += Time.deltaTime;

            if (!wasBurnt && isBurnt)
            {
                if (!fire)
                {
                    smoke = Instantiate(smokePrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                }
                else
                {
                    smoke.SetActive(true);
                }
            }
            if (!wasOnFire && isOnFire)
            {
                if (!fire)
                {
                    fire = Instantiate(firePrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                } else
                {
                    fire.SetActive(true);
                }
            }
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

    private void OnParticleCollision(GameObject other)
    {
        // TODO
        /**
         * Check if these are fire extinguiser particles and if so disable smoke and fire
         *
         * hasBeenExtinguished = true
         */
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
        get => timeCooked >= cookDuration + burnDuration;
    }

    public bool isCooked
    {
        get => timeCooked > cookDuration;
    }

    public bool isOnFire
    {
        get => timeCooked > cookDuration + burnDuration + fireDuration;
    }

    // has been burt, dropped on the floor, touched by the fire extinguisher
    public bool isSpoiled
    {
        get => isBurnt || hasBeenExtinguished || hasDroppedOnFloor;
    }
}
