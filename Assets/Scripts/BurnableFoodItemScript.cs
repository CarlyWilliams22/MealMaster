using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BurnableFoodItemScript : FoodItemScript
{
    public float burnDuration; // time in seconds after the food is fully cooked that it takes to burn it
    public float fireDuration; // time in seconds after food has started to burn until it catches fire
    public GameObject firePrefab;
    public GameObject smokePrefab;

    private GameObject fire;
    private GameObject smoke;
    private bool hasBeenOnFire;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity, transform);
        smoke.SetActive(false);
        fire = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);
        fire.SetActive(false);
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

        if (isCooking)
        {
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

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            if (isBurning && !isOnFire)
            {
                smoke.SetActive(false);
            }
        }
    }

    private new void OnParticleCollision(GameObject other)
    {
        base.OnParticleCollision(other);

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

    public bool isBurning
    {
        get => smoke.activeSelf;
    }

    public bool isOnFire
    {
        get => fire.activeSelf;
    }

    public new bool isSpoiled
    {
        get => hasBeenOnFire || hasDroppedOnFloor;
    }
}
