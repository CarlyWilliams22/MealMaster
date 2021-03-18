using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableScript : MonoBehaviour
{
    private Transform parent;

    public void Grab(GameObject holder, Vector3 offset)
    {
        parent = transform.parent;
        transform.position = holder.transform.position + (transform.position - offset);
        transform.SetParent(holder.transform);
        // TODO enable kinematic and disable gravity for rigidbodies etc
    }

    public bool Release()
    {
        transform.SetParent(parent);
        // TODO disable kinematic and re-enable gravity for rigidbodies etc
        // TODO can the object be released here? If not return false
        return true;
    }
}
