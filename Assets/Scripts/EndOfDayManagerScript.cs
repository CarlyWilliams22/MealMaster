using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class EndOfDayManagerScript : MonoBehaviour
{
    public GameObject MainScreen, Hiring, Restock, Upgrades;
    public Text DayNumberText, Cash;
    ApplicationManagerScript application;

    // Start is called before the first frame update
    void Start()
    {
        DayNumberText.text = "End of Day " +  Prefs.GetDayNumber();
        application = FindObjectOfType<ApplicationManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Cash.text = Prefs.GetCash().ToString("C");
    }

    public void OnClickHiring()
    {
        Hiring.SetActive(true);
        MainScreen.SetActive(false);
    }

    public void OnClickRestock()
    {
        Restock.SetActive(true);
        MainScreen.SetActive(false);
    }

    public void OnClickUpgrades()
    {
        Upgrades.SetActive(true);
        MainScreen.SetActive(false);
    }

    public void OnClickBack()
    {
        MainScreen.SetActive(true);
        Hiring.SetActive(false);
        Restock.SetActive(false);
        Upgrades.SetActive(false);
    }

    public void OnClickStartNextDay()
    {
        Prefs.SetDayNumber(Prefs.GetDayNumber() + 1);
        application.OnClickPlay();
    }

}
