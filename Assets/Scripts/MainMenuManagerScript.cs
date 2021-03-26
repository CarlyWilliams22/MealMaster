using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject MenuButtons, Naming;
    public Text DinerName;


    public void OnStartNewGame()
    {
        Prefs.SetAllToDefault();
        Naming.SetActive(true);
        MenuButtons.SetActive(false);
    }

    public void OnNameEntered()
    {
        Prefs.SetDinerName(DinerName.text);
    }
}
