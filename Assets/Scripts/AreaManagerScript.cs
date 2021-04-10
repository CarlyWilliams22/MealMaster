using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AreaManagerScript : MonoBehaviour
{
    private HashSet<AreaTrackerScript> items;

    void Start()
    {
        items = new HashSet<AreaTrackerScript>();
    }

    public void OnEnter(AreaTrackerScript item)
    {
        items.Add(item);
        Messenger.Broadcast<AreaManagerScript, AreaTrackerScript>(GameEvent.AREA_TRACKER_ENTER_AREA, this, item);
    }

    public void OnExit(AreaTrackerScript item)
    {
        items.Remove(item);
        Messenger.Broadcast<AreaManagerScript, AreaTrackerScript>(GameEvent.AREA_TRACKER_EXIT_AREA, this, item);
    }

    public bool Contains(AreaTrackerScript item)
    {
        return items.Contains(item);
    }

    public GameObject GetItem(string tag, Func<GameObject, bool> condition)
    {
        foreach (AreaTrackerScript item in items)
        {
            if (item.gameObject.tag == tag && condition(item.gameObject))
            {
                return item.gameObject;
            }
        }
        return null;
    }
}
