using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class HUDScript : MonoBehaviour
{
    public Text cashLabel;
    public Text dayNumberLabel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCashLabel(0);
        UpdateDayNumberLabel();
    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_CASH, OnChangedCash);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(GameEvent.CHANGED_CASH, OnChangedCash);
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
}
