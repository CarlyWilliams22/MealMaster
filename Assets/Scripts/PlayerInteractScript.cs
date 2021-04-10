﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour
{
    public LayerMask interactableLayerMask;
    public Camera _camera;
    public float highlightEffectScale;
    public float highlightEffectDuration;
    public GameObject grabPoint;
    public GameObject pointerIndicator;
    public float distance;
    public Vector3 pointerViewportPoint;
    public float clickTimeout;

    private InteractableScript highlighted;
    private HoldableScript holding;
    private float lastClickAndGrabTime;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighlighted();
        UpdateHolding();

        if (Input.GetMouseButtonDown(0))
        {
            if (highlighted)
            {
                Messenger.Broadcast(GameEvent.CLICK_INTERACTABLE, highlighted);
            }
        }
    }

    private void UpdateHighlighted()
    {
        if (!holding)
        {
            Vector3 pointerInWorld = _camera.ViewportToWorldPoint(pointerViewportPoint);
            RaycastHit hit;
            /**
             * manually check distance instead of limiting the distance of the raycast because
             * we want to highlight the object if the closest point on the hit object is within 
             * the distance range even if the actual hit point of the ray cast is out of the range
             * 
             * for performance we will limit the raycast range to an arbitrary value beyond the
             * distance range as long as this limit is large enough such that if the closest point
             * on a grabbable object is within the distance then all points on that object should
             * be within the raycast range
             */
            bool hitSuccess = Physics.Raycast(_camera.ViewportPointToRay(pointerViewportPoint), out hit, distance + 5);
            bool correctLayer = hitSuccess && (interactableLayerMask == (interactableLayerMask | (1 << hit.collider.gameObject.layer)));
            bool withinDistance = hitSuccess && correctLayer && Vector3.Distance(hit.collider.ClosestPoint(pointerInWorld), pointerInWorld) <= distance;
            if (withinDistance)
            {
                InteractableScript interactable = hit.collider.gameObject.GetComponent<InteractableScript>();
                if (interactable && interactable.interactionEnabled)
                {
                    if (interactable != highlighted)
                    {
                        if (highlighted && highlighted.shrinkCoroutine == null)
                        {
                            UnHighlight(highlighted);
                            highlighted = null;
                        }
                        Highlight(interactable);
                        highlighted = interactable;
                    }
                }
            }
            else
            {
                if (highlighted && highlighted.shrinkCoroutine == null)
                {
                    UnHighlight(highlighted);
                    highlighted = null;
                }
            }
        }
    }

    private void UpdateHolding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (holding)
            {
                if (Time.time - lastClickAndGrabTime > clickTimeout)
                {
                    ReleaseHolding();
                }
            } 
            else
            {
                if (highlighted)
                {
                    if (highlighted.GetComponent<InventoryItemScript>())
                    {
                        InventoryItemScript inventoryItem = highlighted.GetComponent<InventoryItemScript>();
                        if (inventoryItem.CanInstantiateItem())
                        {
                            holding = inventoryItem.InstantiateItem().GetComponent<HoldableScript>();
                            UnHighlight(highlighted);
                            highlighted = holding.GetComponent<InteractableScript>();
                            Highlight(highlighted);
                        }
                    } 
                    else
                    {
                        holding = highlighted.gameObject.GetComponent<HoldableScript>();
                    }
                    if (holding)
                    {
                        Vector3 offset = holding.GetComponent<Collider>().ClosestPoint(_camera.ViewportToWorldPoint(pointerViewportPoint));
                        holding.Grab(grabPoint, offset);
                        pointerIndicator.SetActive(false);
                        lastClickAndGrabTime = Time.time;
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (holding && Time.time - lastClickAndGrabTime > clickTimeout)
            {
                ReleaseHolding();
            }
            lastClickAndGrabTime = 0;
        }   
    }

    public void ReleaseHolding()
    {
        if (holding.Release())
        {
            holding = null;
            pointerIndicator.SetActive(true);
        }
    }

    private void Highlight(InteractableScript interactable)
    {
        if (interactable.shrinkCoroutine != null)
        {
            UnHighlight(interactable);
        }
        if (interactable.shrinkCoroutine == null && interactable.growCoroutine == null)
        {
            interactable.preEffectScale = interactable.transform.localScale;
        }
        interactable.growCoroutine = HighlightEffect(interactable, Time.time, highlightEffectDuration, interactable.transform.localScale, interactable.transform.localScale * highlightEffectScale, () => interactable.growCoroutine = null, () => interactable.growCoroutine == null);
        StartCoroutine(interactable.growCoroutine);
    }

    private void UnHighlight(InteractableScript interactable)
    {
        if (interactable.growCoroutine != null)
        {
            StopCoroutine(interactable.growCoroutine);
            interactable.growCoroutine = null;
        }
        if (interactable.shrinkCoroutine == null)
        {
            interactable.shrinkCoroutine = HighlightEffect(interactable, Time.time, highlightEffectDuration, interactable.transform.localScale, interactable.preEffectScale, () => interactable.shrinkCoroutine = null, () => interactable.shrinkCoroutine == null);
            StartCoroutine(interactable.shrinkCoroutine);
        }
    }

    private IEnumerator HighlightEffect(InteractableScript interactable, float startTime, float duration, Vector3 originalScale, Vector3 newScale, Action onDone, Func<bool> isStopped)
    {
        while (Time.time - startTime <= duration && !isStopped())
        {
            float lerp = (Time.time - startTime) / duration;
            Vector3 lerpScale = new Vector3(Mathf.Lerp(originalScale.x, newScale.x, lerp), Mathf.Lerp(originalScale.y, newScale.y, lerp), Mathf.Lerp(originalScale.z, newScale.z, lerp));
            interactable.gameObject.transform.localScale = lerpScale;
            yield return new WaitForEndOfFrame();
        }
        if (!isStopped())
        {
            interactable.gameObject.transform.localScale = newScale;
            onDone();
        }
    }
}
