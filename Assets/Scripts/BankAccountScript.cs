using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class BankAccountScript : MonoBehaviour
{
    public float currentBalance;
    public float balanceAtLevelStart;

    private static BankAccountScript _instance = null;

    public static BankAccountScript Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    private void OnEnable()
    {
        balanceAtLevelStart = Prefs.GetCash();
        currentBalance = balanceAtLevelStart;
    }

    private void OnDisable()
    {
        Prefs.SetCash(currentBalance);
        Prefs.SetLevelProfit(currentBalance - balanceAtLevelStart);
    }

    public float GetProfit()
    {
        return currentBalance - balanceAtLevelStart;
    }

    public void SetBalanceAtLevelStart(float startingBalance)
    {
        balanceAtLevelStart = startingBalance;
    }

    public void CreditAccount(float credit)
    {
        currentBalance += credit;
        Messenger.Broadcast(GameEvent.CHANGED_CASH, currentBalance);
    }

    public void WithdrawFromAccount(float withdrawal)
    {
        currentBalance -= withdrawal;
        Messenger.Broadcast(GameEvent.CHANGED_CASH, currentBalance);
    }

}
