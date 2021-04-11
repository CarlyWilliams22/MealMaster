using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System;

public class EndOfDayManagerScript : MonoBehaviour
{
    public GameObject MainScreen, Hiring, Restock, Upgrades;
    public Text DayNumberText, ProfitAndBalance, CustomerReview;
    float employeeWage = 7.25f;
    float profit, tips, popularity;
    string diner;
    int numCustomers;

    // Start is called before the first frame update
    void Start()
    {
        Prefs.SetEmployeeHired(false);
        Prefs.SetCurrentScene("EndOfDayScene");
        DayNumberText.text = "End of Day " + Prefs.GetDayNumber();
        diner = Prefs.GetDinerName();
        profit = Prefs.GetLevelProfit();
        tips = Prefs.GetLevelTips();
        numCustomers = Prefs.GetLevelCustomersServed();
        ProfitAndBalance.text = "Day's Profit: " + profit.ToString("C") + "   Current Balance: " + Prefs.GetCash().ToString("C");

        popularity = 4* tips / numCustomers;
        
        if (popularity < .15)
        {
            BadCustomerReview();
        }
        else if (popularity < .4)
        {
            EhCustomerReview();
        }
        else
        {
            GoodCustomerReview();
        }
        Mathf.Clamp(popularity, 0, 1);
        Prefs.SetPopularityScore(popularity);
    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_CASH, OnCashChange);
    }

    private void OnDisable()
    {
        
        Messenger.RemoveListener<float>(GameEvent.CHANGED_CASH, OnCashChange);
    }

    private void BadCustomerReview()
    {
        CustomerReview.text = "Man, " + diner + " has terrible service. Do not recommend.";
    }

    private void EhCustomerReview()
    {
        CustomerReview.text = "This place is decent.";
    }

    private void GoodCustomerReview()
    {
        CustomerReview.text = diner + " is the best restaurant I have been to in a long time! Their food is awesome!";
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
        ProfitAndBalance.text = "Day's Profit: " + profit.ToString("C") + "   Current Balance: " + value.ToString("C");
    }

    public void OnClickHireWorker()
    {
        if (!Prefs.GetEmployeeHired())
        {
            float updatedCash = Prefs.GetCash() - employeeWage;
            if (updatedCash >= 0)
            {
                Prefs.SetEmployeeHired(true);
                Prefs.SetCash(updatedCash);
                OnCashChange(updatedCash);
            }
        }
    }
}
