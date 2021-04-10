﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public IEnumerator growCoroutine;
    public IEnumerator shrinkCoroutine;
    public Vector3 preEffectScale;
    public bool interactionEnabled;

    // Start is called before the first frame update
    void Start()
    {
        preEffectScale = transform.localScale;
    }

    public void CancelEffects()
    {
        if (growCoroutine != null)
        {
            StopCoroutine(growCoroutine);
            growCoroutine = null;
        }
        if (shrinkCoroutine != null)
        {
            StopCoroutine(shrinkCoroutine);
            shrinkCoroutine = null;
        }
    }
}
