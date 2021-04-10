using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class EmployeeManagerScript : MonoBehaviour
{
    public GameObject employee;

    // Start is called before the first frame update
    void Start()
    {
        if (Prefs.GetEmployeeHired())
        {
            employee.SetActive(true);
        }
        else
        {
            employee.SetActive(false);
        }
    }

}
