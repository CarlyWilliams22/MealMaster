using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RestockInventoryScript : MonoBehaviour
{
    public Text moneyCalcLabel;
    public Text orderAmountLabel;
    float restockCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeOrderAmount()
    {
        float price = FoodItemScript.wholesalePrices[FoodItemScript.FoodItemType.BURGER];
        restockCost = int.Parse(orderAmountLabel.text) * price;
        moneyCalcLabel.text = "x " + price.ToString("C") + " = " + restockCost.ToString("C");
    }

    public void OnClickPlaceOrder()
    {
        BankAccountScript.Instance.WithdrawFromAccount(restockCost);
    }
}
