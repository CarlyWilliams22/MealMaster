using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrinkScript : FoodItemScript
{
    public GameObject straw;

    private GameObject drinkSodaMachineFillArea;
    private bool drinkNotifySodaMachineFillAreaEnter;


    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (holdable.isHeld())
        {
            if (drinkSodaMachineFillArea)
            {
                drinkSodaMachineFillArea.GetComponentInParent<SodaMachineScript>().OnDrinkExitFillArea(drinkSodaMachineFillArea, gameObject);
                drinkSodaMachineFillArea = null;
            }
        }
        else
        {
            if (drinkNotifySodaMachineFillAreaEnter && drinkSodaMachineFillArea)
            {
                drinkSodaMachineFillArea.GetComponentInParent<SodaMachineScript>().OnDrinkEnterFillArea(drinkSodaMachineFillArea, gameObject);
                drinkNotifySodaMachineFillAreaEnter = false;
            }
        }

        if (!straw.activeSelf && isCooked)
        {
            straw.SetActive(true);
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (cookerTags.Any(x => other.gameObject.CompareTag(x)))
        {
            drinkSodaMachineFillArea = other.gameObject;
            drinkNotifySodaMachineFillAreaEnter = true;
        }
    }
}
