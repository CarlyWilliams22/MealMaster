using System.Collections;
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
        { FoodItemType.BUN, 0.15f }
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
    public List<string> cookerTags; // list of tags of gameobjects than can cook this food item when within their trigger collider

    protected bool hasDroppedOnFloor;
    private HashSet<Collider> cookingColliders;
    protected HoldableScript holdable;

    public void Awake()
    {
        cookingColliders = new HashSet<Collider>();
        holdable = GetComponent<HoldableScript>();
    }

    public virtual void Update()
    {
        if (isCooking)
        {
            bool wasCooked = isCooked;
            timeCooked += Time.deltaTime;
            bool isNowCooked = isCooked;

            if (!wasCooked && isNowCooked)
            {
                Messenger.Broadcast(GameEvent.FOOD_ITEM_COOKED, this);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            cookingColliders.Add(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            cookingColliders.Remove(other);
        }
    }

    public bool isCooking
    {
        get => cookingColliders.Count > 0 && !holdable.isHeld();
    }

    public void OnParticleCollision(GameObject other)
    {
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

    public virtual bool isSpoiled
    {
        get => hasDroppedOnFloor;
    }

    public bool isCooked
    {
        get => timeCooked >= cookDuration;
    }

    public virtual bool isReadyToServe
    {
        get => isCooked && !isSpoiled;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            hasDroppedOnFloor = true;
        }
    }
}
