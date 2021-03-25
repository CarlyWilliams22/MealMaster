using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherScript : MonoBehaviour
{
    public GameObject water;

    private void OnEnable()
    {
        Messenger.AddListener<HoldableScript, bool>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<HoldableScript, bool>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
    }

    private void OnGrabHoldable(HoldableScript holdable, bool grabbed)
    {
        if (holdable.gameObject == gameObject)
        {
            water.SetActive(grabbed);
        }
    }
}
