using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotScript : MonoBehaviour
{

    public CustomerScript occupier;

    public bool isOpen {
        get => occupier == null; 
    }

}
