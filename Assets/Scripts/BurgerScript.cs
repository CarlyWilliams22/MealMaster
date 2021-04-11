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
    public AudioClip cookingAudio, burningAudio;

    public Material burntMaterial;
    public Material cookedMaterial;
    public Material spoiledMaterial;

    private GameObject topBun;
    private GameObject bottomBun;
    private AreaTrackerScript areaTracker;
    AudioSource audioPlayer;
    bool alarmIsPlaying, cooking, burning = false;
    private MeshRenderer _renderer;

    private bool wasOnFire;
    private bool wasHasDroppedOnFloor;
    private bool wasCooked;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        areaTracker = GetComponent<AreaTrackerScript>();
        audioPlayer = GetComponent<AudioSource>();
        _renderer = GetComponent<MeshRenderer>();
    }

    public override void Update()
    {
        base.Update();

        if (isCooking && !cooking)
        {
            audioPlayer.PlayOneShot(cookingAudio);
            cooking = true;
        }
        else if(!isCooking && cooking)
        {
            audioPlayer.Stop();
            cooking = false;
        }

        if(!audioPlayer.isPlaying && isBurning)
        {
            burning = false;
        }

        if(isBurning && !burning)
        {
            audioPlayer.PlayOneShot(burningAudio);
            burning = true;
        }
        else if(!isBurning && burning)
        {
            audioPlayer.Stop();
            burning = false;
        }

        if (isOnFire && !alarmIsPlaying)
        {
            Messenger.Broadcast(GameEvent.ON_FIRE, GetComponent<BurnableFoodItemScript>());
            alarmIsPlaying = true;
        }
        else if(!isOnFire && alarmIsPlaying)
        {
            Messenger.Broadcast(GameEvent.OFF_FIRE, GetComponent<BurnableFoodItemScript>());
            alarmIsPlaying = false;
        }

        if (!wasOnFire && isOnFire)
        {
            _renderer.material = burntMaterial;
        }
        if (!wasCooked && isCooked) {
            _renderer.material = cookedMaterial;
        }
        if (!wasHasDroppedOnFloor && hasDroppedOnFloor)
        {
            if (!hasBeenOnFire)
            {
                _renderer.material = spoiledMaterial;
            }
        }

        wasCooked = isCooked;
        wasOnFire = isOnFire;
        wasHasDroppedOnFloor = hasDroppedOnFloor;
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
        if (isCooked && !isSpoiled)
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

    public override bool isReadyToServe
    {
        get => base.isReadyToServe && !isSpoiled && topBun && bottomBun && !topBun.GetComponent<FoodItemScript>().isSpoiled && !bottomBun.GetComponent<FoodItemScript>().isSpoiled;
    }

}
