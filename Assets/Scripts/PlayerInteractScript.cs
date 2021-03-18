using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour
{
    public LayerMask interactableLayer;
    public Camera _camera;
    public float highlightEffectScale;
    public float highlightEffectDuration;

    private InteractableScript highlighted;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, Mathf.Infinity, interactableLayer))
        {
            InteractableScript interactable = hit.collider.gameObject.GetComponent<InteractableScript>();
            if (interactable)
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
        interactable.growCoroutine = HighlightEffect(interactable, Time.time, highlightEffectDuration, interactable.transform.localScale, interactable.transform.localScale * highlightEffectScale, () => interactable.growCoroutine = null);
        StartCoroutine(interactable.growCoroutine);
    }

    private void UnHighlight(InteractableScript interactable)
    {
        if (interactable.growCoroutine != null)
        {
            StopCoroutine(interactable.growCoroutine);
            interactable.growCoroutine = null;
        }
        interactable.growCoroutine = HighlightEffect(interactable, Time.time, highlightEffectDuration, interactable.transform.localScale, interactable.preEffectScale, () => interactable.shrinkCoroutine = null);
        StartCoroutine(interactable.growCoroutine);
    }

    private IEnumerator HighlightEffect(InteractableScript interactable, float startTime, float duration, Vector3 originalScale, Vector3 newScale, Action onDone)
    {
        while (Time.time - startTime <= duration)
        {
            float lerp = (Time.time - startTime) / duration;
            Vector3 lerpScale = new Vector3(Mathf.Lerp(originalScale.x, newScale.x, lerp), Mathf.Lerp(originalScale.y, newScale.y, lerp), Mathf.Lerp(originalScale.z, newScale.z, lerp));
            interactable.gameObject.transform.localScale = lerpScale;
            yield return new WaitForEndOfFrame();
        }
        interactable.gameObject.transform.localScale = newScale;
        onDone();
    }
}
