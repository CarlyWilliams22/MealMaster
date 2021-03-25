using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class HUDScript : MonoBehaviour
{
    public Text cashLabel;
    public Text dayNumberLabel;
    public Text timeOfDayLabel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCashLabel(0);
        UpdateDayNumberLabel();
    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_CASH, OnChangedCash);
        Messenger.AddListener<int>(GameEvent.CHANGED_TIME_OF_DAY, OnChangedTimeOfDay);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(GameEvent.CHANGED_CASH, OnChangedCash);
        Messenger.RemoveListener<int>(GameEvent.CHANGED_TIME_OF_DAY, OnChangedTimeOfDay);
    }

    private void OnChangedCash(float cash)
    {
        UpdateCashLabel(BankAccountScript.Instance.currentBalance);
    }

    private void UpdateCashLabel(float cash)
    {
        cashLabel.text = cash.ToString("C");
    }

    private void UpdateDayNumberLabel()
    {
        dayNumberLabel.text = "Day " + Prefs.GetDayNumber();
    }

    private void OnChangedTimeOfDay(int time)
    {
        timeOfDayLabel.text = time + ":00";
    }
}
