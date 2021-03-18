using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankAccountScript : MonoBehaviour
{
    public float currentBalance;
    public float balanceAtLevelStart;


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
    }

    public void WithdrawFromAccount(float withdrawal)
    {
        currentBalance -= withdrawal;
    }

}
