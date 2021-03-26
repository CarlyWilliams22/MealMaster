using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class RestockInventoryScript : MonoBehaviour
{
    public Text moneyCalcLabel;
    public Text orderAmountLabel;
    public Text currentBurgerInventory;
    public Text cash;
    float restockCost;
    int orderAmount;

    // Start is called before the first frame update
    void Start()
    {
        currentBurgerInventory.text = "BURGERS: (Current Inventory: " + Prefs.GetInventoryBurger() + ")";
        cash.text = Prefs.GetCash().ToString("C");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChangeOrderAmount()
    {
        float price = FoodItemScript.wholesalePrices[FoodItemScript.FoodItemType.BURGER];
        orderAmount = int.Parse(orderAmountLabel.text);
        restockCost = orderAmount * price;
        moneyCalcLabel.text = "x " + price.ToString("C") + " = " + restockCost.ToString("C");
    }

    public void OnClickPlaceOrder()
    {
        //bankAccount.WithdrawFromAccount(restockCost);
        Prefs.SetCash(Prefs.GetCash() - restockCost);
        Prefs.SetInventoryBurger(Prefs.GetInventoryBurger() + orderAmount);
        Messenger.Broadcast(GameEvent.CHANGED_INVENTORY_BURGER);
        Messenger.Broadcast(GameEvent.CHANGED_CASH, Prefs.GetCash());
        UpdatedCash();
        UpdatedInventoryBurger();
    }

    private void UpdatedInventoryBurger()
    {
        currentBurgerInventory.text = "BURGERS: (Current Inventory: " + Prefs.GetInventoryBurger() + ")";
    }

    private void UpdatedCash()
    {
        cash.text = Prefs.GetCash().ToString("C");
    }
}
