﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodItemScript : MonoBehaviour
{
    public enum FoodItemType {
        DRINK, BURGER, BUN
    }

    // restocking prices
    public static readonly Dictionary<FoodItemType, float> wholesalePrices = new Dictionary<FoodItemType, float>
    {
        { FoodItemType.DRINK, 0.25f },
        { FoodItemType.BURGER, 0.50f },
        { FoodItemType.BUN, 0.25f }
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

    private GameObject fire;
    private GameObject smoke;
    private bool hasDroppedOnFloor;
    private bool hasBeenOnFire;
    private HashSet<Collider> cookingColliders;
    private HoldableScript holdable;

    private void Start()
    {
        smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity, transform);
        smoke.SetActive(false);
        fire = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);
        fire.SetActive(false);
        cookingColliders = new HashSet<Collider>();
        holdable = GetComponent<HoldableScript>();
    }

    private void Update()
    {
        if (isCooking)
        {
            timeCooked += Time.deltaTime;

            if (!isBurning && timeCooked >= cookDuration + burnDuration)
            {
                smoke.SetActive(true);
            }
            if (!isOnFire && timeCooked > cookDuration + burnDuration + fireDuration)
            {
                fire.SetActive(true);
                hasBeenOnFire = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            cookingColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            cookingColliders.Remove(other);

            if (isBurning && !isOnFire)
            {
                smoke.SetActive(false);
            }
        }
    }

    public bool isCooking
    {
        get => cookingColliders.Count > 0 && !holdable.isHeld();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isBurning)
        {
            smoke.SetActive(false);
        }
        if (isOnFire)
        {
            fire.SetActive(false);
        }
        if (timeCooked > cookDuration)
        {
            timeCooked = cookDuration;
        }
    }

    public float retailPrice
    {
        get => retailPrices[type];
    }

    public float WholesalePrice
    {
        get => wholesalePrices[type];
    }

    public bool isBurning
    {
        get => smoke.activeSelf;
    }

    public bool isOnFire
    {
        get => fire.activeSelf;
    }

    public bool isSpoiled
    {
        get => hasBeenOnFire || hasDroppedOnFloor;
    }
}
