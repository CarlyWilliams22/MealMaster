using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableScript : MonoBehaviour
{
    private Transform parent;
    private Rigidbody rbody;

    private bool origIsKinematic;
    private bool origUseGravity;

    private void Start()
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
    }

    public bool Release()
    {
        transform.SetParent(parent);
        if (rbody)
        {
            rbody.isKinematic = origIsKinematic;
            rbody.useGravity = origUseGravity;
        }
        // TODO can the object be released here? If not return false
        return true;
    }
}
