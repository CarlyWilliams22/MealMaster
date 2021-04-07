using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotScript : MonoBehaviour
{

    private bool open = true;

    public void taken()
    {
        open = false;
    }

    public void free()
    {
        open = true;
    }

    public bool getOpen() { return open; }

}
