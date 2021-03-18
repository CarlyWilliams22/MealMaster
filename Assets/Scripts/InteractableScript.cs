using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public IEnumerator growCoroutine;
    public IEnumerator shrinkCoroutine;
    public Vector3 preEffectScale;

    // Start is called before the first frame update
    void Start()
    {
        preEffectScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
