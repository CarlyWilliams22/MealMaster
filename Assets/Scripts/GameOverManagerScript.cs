using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class GameOverManagerScript : MonoBehaviour
{
    public Text GameStats;

    // Start is called before the first frame update
    void Start()
    {
        int numDays = Prefs.GetDayNumber();
        if (numDays == 1)
        {
            GameStats.text = "Sadly," + Prefs.GetDinerName() + " has closed after 1 day of service.";
        }
        else
        {
            GameStats.text = "Sadly," + Prefs.GetDinerName() + " has closed after " + numDays + " days of service.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
