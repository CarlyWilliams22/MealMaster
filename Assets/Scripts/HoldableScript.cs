using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableScript : MonoBehaviour
{
    private Transform parent;
    private Rigidbody rbody;

    private bool origIsKinematic;
    private bool origUseGravity;
    private bool _isHeld;

    public Vector3 grabRotation;
    public bool useGrabRotation;
    public float releaseRotationX;
    public bool useReleaseRotationX;

    /**
     * In the case of Instantiating a Holdable and then immediately grabbing it, 
     * Grab() may be called before Start(), so setup stuff needs to be done prior to that in Awake
     */
    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    public void Grab(GameObject holder, Vector3 offset)
    {
        parent = transform.parent;
        transform.position = holder.transform.position + (transform.position - offset);
        transform.SetParent(holder.transform);
        if (rbody)
        {
            origIsKinematic = rbody.isKinematic;
            rbody.isKinematic = true;
            origUseGravity = rbody.useGravity;
            rbody.useGravity = false;
        }
        if (useGrabRotation)
        {
            transform.localEulerAngles = grabRotation;
        }
        _isHeld = true;
        Messenger.Broadcast(GameEvent.GRAB_HOLDABLE, this, true);
    }

    public bool Release()
    {
        transform.SetParent(parent);
        if (rbody)
        {
            rbody.isKinematic = origIsKinematic;
            rbody.useGravity = origUseGravity;
        }

        if (useReleaseRotationX)
        {
            transform.localEulerAngles = new Vector3(releaseRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        // TODO can the object be released here? If not return false
        _isHeld = false;
        if (!_isHeld)
        {
            Messenger.Broadcast(GameEvent.GRAB_HOLDABLE, this, false);
        }
        return !_isHeld;
    }

    public bool isHeld()
    {
        return _isHeld;
    }
}
