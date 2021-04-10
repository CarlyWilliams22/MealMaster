using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class RestockInventoryScript : MonoBehaviour
{
    public Text burgerMoneyCalcLabel, bunMoneyCalcLabel, cupMoneyCalcLabel;
    public Text orderBurgerAmountLabel, orderBunAmountLabel, orderCupAmountLabel;
    public Text currentBurgerInventory, currentBunInventory, currentCupInventory;
    public Text cash;
    float restockBurgerCost, restockBunCost, restockCupCost;
    int orderBurgerAmount, orderBunAmount, orderCupAmount;

    // Start is called before the first frame update
    void Start()
    {
        UpdatedInventoryBurger();
        UpdatedInventoryBun();
        UpdatedInventoryCup();
        OnChangeOrderAmount(FoodItemScript.FoodItemType.BURGER);
        OnChangeOrderAmount(FoodItemScript.FoodItemType.BUN);
        OnChangeOrderAmount(FoodItemScript.FoodItemType.DRINK);
        cash.text = Prefs.GetCash().ToString("C");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnChangeOrderAmount(FoodItemScript.FoodItemType food)
    {
        float price;
        if (food == FoodItemScript.FoodItemType.BURGER)
        {
            price = FoodItemScript.wholesalePrices[FoodItemScript.FoodItemType.BURGER];
            orderBurgerAmount = int.Parse(orderBurgerAmountLabel.text);
            restockBurgerCost = orderBurgerAmount * price;
            burgerMoneyCalcLabel.text = "x " + price.ToString("C") + " = " + restockBurgerCost.ToString("C");
        }
        if (food == FoodItemScript.FoodItemType.BUN)
        {
            price = FoodItemScript.wholesalePrices[FoodItemScript.FoodItemType.BUN];
            orderBunAmount = int.Parse(orderBunAmountLabel.text);
            restockBunCost = orderBunAmount * price;
            bunMoneyCalcLabel.text = "x " + price.ToString("C") + " = " + restockBunCost.ToString("C");
        }
        if (food == FoodItemScript.FoodItemType.DRINK)
        {
            price = FoodItemScript.wholesalePrices[FoodItemScript.FoodItemType.DRINK];
            orderCupAmount = int.Parse(orderCupAmountLabel.text);
            restockCupCost = orderCupAmount * price;
            cupMoneyCalcLabel.text = "x " + price.ToString("C") + " = " + restockCupCost.ToString("C");
        }
    }

    public void OnChangeBurgerOrderAmount()
    {
        OnChangeOrderAmount(FoodItemScript.FoodItemType.BURGER);
    }

    public void OnChangeBunOrderAmount()
    {
        OnChangeOrderAmount(FoodItemScript.FoodItemType.BUN);
    }

    public void OnChangeCupOrderAmount()
    {
        OnChangeOrderAmount(FoodItemScript.FoodItemType.DRINK);
    }

    private void OnClickPlaceOrder(FoodItemScript.FoodItemType food)
    {
        //bankAccount.WithdrawFromAccount(restockCost);
        if (food == FoodItemScript.FoodItemType.BURGER)
        {
            if (Prefs.GetCash() - restockBurgerCost >= 0)
            {
                Prefs.SetCash(Prefs.GetCash() - restockBurgerCost);
                Prefs.SetInventoryBurger(Prefs.GetInventoryBurger() + orderBurgerAmount);
                Messenger.Broadcast(GameEvent.CHANGED_CASH, Prefs.GetCash());
                UpdatedCash();
                UpdatedInventoryBurger();
            }
        }
        if (food == FoodItemScript.FoodItemType.BUN)
        {
            if (Prefs.GetCash() - restockBunCost >= 0)
            {
                Prefs.SetCash(Prefs.GetCash() - restockBunCost);
                Prefs.SetInventoryBun(Prefs.GetInventoryBun() + orderBunAmount);
                Messenger.Broadcast(GameEvent.CHANGED_CASH, Prefs.GetCash());
                UpdatedCash();
                UpdatedInventoryBun();
            }
        }
        if (food == FoodItemScript.FoodItemType.DRINK)
        {
            if (Prefs.GetCash() - restockCupCost >= 0)
            {
                Prefs.SetCash(Prefs.GetCash() - restockCupCost);
                Prefs.SetInventoryCup(Prefs.GetInventoryCup() + orderCupAmount);
                Messenger.Broadcast(GameEvent.CHANGED_CASH, Prefs.GetCash());
                UpdatedCash();
                UpdatedInventoryCup();
            }
        }
    }

    public void OnClickPlaceBurgerOrder()
    {
        OnClickPlaceOrder(FoodItemScript.FoodItemType.BURGER);
    }

    public void OnClickPlaceBunOrder()
    {
        OnClickPlaceOrder(FoodItemScript.FoodItemType.BUN);
    }

    public void OnClickPlaceCupOrder()
    {
        OnClickPlaceOrder(FoodItemScript.FoodItemType.DRINK);
    }


    private void UpdatedInventoryBurger()
    {
        currentBurgerInventory.text = "BURGERS: (Current Inventory: " + Prefs.GetInventoryBurger() + ")";
    }

    private void UpdatedInventoryBun()
    {
        currentBunInventory.text = "BUNS: (Current Inventory: " + Prefs.GetInventoryBun() + ")";
    }

    private void UpdatedInventoryCup()
    {
        currentCupInventory.text = "CUPS: (Current Inventory: " + Prefs.GetInventoryCup() + ")";
    }

    private void UpdatedCash()
    {
        cash.text = Prefs.GetCash().ToString("C");
    }
}
