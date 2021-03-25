using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RestockInventoryScript : MonoBehaviour
{
    public Text moneyCalcLabel;
    public Text orderAmountLabel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onChangeOrderAmount()
    {
        float price = FoodItemScript.wholesalePrices[FoodItemScript.FoodItemType.BURGER];
        moneyCalcLabel.text = "x " + price.ToString("C") + " = " + (int.Parse(orderAmountLabel.text) * price).ToString("C");
    }
}
