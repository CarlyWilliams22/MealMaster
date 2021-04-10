using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AreaTrackerScript : MonoBehaviour
{
    private HashSet<AreaManagerScript> areas;
    private HoldableScript holdable;

    private Action onReleaseHoldable;

    public string[] areaTags;


    private void Start()
    {
        areas = new HashSet<AreaManagerScript>();
        holdable = GetComponent<HoldableScript>();
    }

    private void OnEnable()
    {
        Messenger.AddListener<HoldableScript, bool>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<HoldableScript, bool>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (areaTags.Any(x => other.gameObject.CompareTag(x)))
        {
            AreaManagerScript am = other.gameObject.GetComponent<AreaManagerScript>();

            if (holdable && holdable.isHeld())
            {
                onReleaseHoldable = () =>
                {
                    areas.Add(am);
                    am.OnEnter(this);
                };
            } else
            {
                areas.Add(am);
                am.OnEnter(this);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (areaTags.Any(x => other.gameObject.CompareTag(x)))
        {
            AreaManagerScript am = other.gameObject.GetComponent<AreaManagerScript>();
            areas.Remove(am);
            am.OnExit(this);
        }
    }

    public static bool InSameArea(AreaTrackerScript a, AreaTrackerScript b)
    {
        return a.areas.Any(x => x.Contains(b));
    }

    private void OnGrabHoldable(HoldableScript h, bool grabbed)
    {
        if (h == holdable && !grabbed && onReleaseHoldable != null)
        {
            onReleaseHoldable();
            onReleaseHoldable = null;
        }
    }

    public void SelfDestruct() { 
        foreach (AreaManagerScript area in areas)
        {
            area.OnExit(this);
        }
        Destroy(this);
    }
}
