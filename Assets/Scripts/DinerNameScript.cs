using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class DinerNameScript : MonoBehaviour
{
    TextMesh DinerName;

    // Start is called before the first frame update
    void Start()
    {
        DinerName = GetComponent<TextMesh>();
        DinerName.text = Prefs.GetDinerName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
