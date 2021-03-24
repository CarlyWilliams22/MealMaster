using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDayManagerScript : MonoBehaviour
{
    public GameObject MainScreen, Hiring, Restock, Upgrades;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}
