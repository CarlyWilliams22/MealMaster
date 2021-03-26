using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class EndOfDayManagerScript : MonoBehaviour
{
    public GameObject MainScreen, Hiring, Restock, Upgrades;
    public Text DayNumberText, ProfitAndBalance;

    // Start is called before the first frame update
    void Start()
    {
        DayNumberText.text = "End of Day " + Prefs.GetDayNumber();
        ProfitAndBalance.text = "Day's Profit: " + Prefs.GetLevelProfit().ToString("C") + "   Current Balance: " + Prefs.GetCash().ToString("C");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_CASH, OnCashChange);
    }

    private void OnDisable()
    {
        Prefs.SetDayNumber(Prefs.GetDayNumber() + 1);
        Messenger.RemoveListener<float>(GameEvent.CHANGED_CASH, OnCashChange);
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

    private void OnCashChange(float value)
    {
        ProfitAndBalance.text = "Day's Profit: " + Prefs.GetLevelProfit().ToString("C") + "   Current Balance: " + Prefs.GetCash().ToString("C");
    }

}
