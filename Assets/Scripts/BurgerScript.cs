using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerScript : BurnableFoodItemScript
{
    public Transform topBunPosition;
    public Transform bottomBunPosition;
    public Vector3 topBunRotation;
    public Vector3 bottomBunRotation;
    public Vector3 bunScale;

    private GameObject topBun;
    private GameObject bottomBun;
    private AreaTrackerScript areaTracker;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        areaTracker = GetComponent<AreaTrackerScript>();
    }

    private void OnEnable()
    {
        Messenger.AddListener<AreaManagerScript, AreaTrackerScript>(GameEvent.AREA_TRACKER_ENTER_AREA, OnAreaTrackerEnterArea);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<AreaManagerScript, AreaTrackerScript>(GameEvent.AREA_TRACKER_ENTER_AREA, OnAreaTrackerEnterArea);
    }

    private void OnAreaTrackerEnterArea(AreaManagerScript area, AreaTrackerScript item)
    {
        if (area.gameObject.tag == "TableTopArea")
        {
            if (item.gameObject.tag == "BurgerBun" && (!item.gameObject.transform.parent || item.gameObject.transform.parent.tag != "Burger"))
            {
                if (area.Contains(areaTracker)) // is the burger in this area?
                {
                    AddBun(item.gameObject);
                }
            }
            else if (item.gameObject == gameObject) // burger entered the area
            {
                GameObject bun = area.GetItem("BurgerBun", (GameObject b) => !item.gameObject.transform.parent || item.gameObject.transform.parent.tag != "Burger"); // try to get a bun on the table
                if (bun)
                {
                    AddBun(bun);
                }
            }
        }
    }

    private void AddBun(GameObject bun)
    {
        if (isCooked)
        {
            bool added = false;
            if (!bottomBun)
            {
                bun.transform.SetParent(gameObject.transform);
                bun.transform.position = bottomBunPosition.position;
                bun.transform.eulerAngles = bottomBunRotation;
                bottomBun = bun;
                added = true;
            }
            else if (!topBun)
            {
                bun.transform.SetParent(gameObject.transform);
                bun.transform.position = topBunPosition.position;
                bun.transform.eulerAngles = topBunRotation;
                topBun = bun;
                added = true;

            }
            if (added)
            {
                InteractableScript interactable = bun.GetComponent<InteractableScript>();
                interactable.interactionEnabled = false;
                interactable.CancelEffects();
                bun.transform.localScale = bunScale;
                bun.layer = LayerMask.NameToLayer("Ignore Raycast");
                bun.GetComponent<Rigidbody>().isKinematic = true;
                bun.GetComponent<AreaTrackerScript>().SelfDestruct();
                bun.GetComponent<MeshCollider>().convex = false;
                bun.GetComponent<Rigidbody>().detectCollisions = false;
            }
        }
    }
}
