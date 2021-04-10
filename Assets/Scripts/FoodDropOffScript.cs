using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDropOffScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        FoodItemScript food = collision.gameObject.GetComponent<FoodItemScript>();
        if (food)
        {
            CustomerScript customer = FindNearestCustomer();
            if (customer)
            {
                if (customer.order == food.type && food.isReadyToServe)
                {
                    food.GetComponent<InteractableScript>().interactionEnabled = false;
                    Vector3 offset = food.GetComponent<Collider>().ClosestPoint(gameObject.transform.position);
                    food.GetComponent<HoldableScript>().Grab(customer.grabPoint, offset);
                    FoodManagerScript.Instance.RemoveItem(food);
                    customer.holding = food.GetComponent<HoldableScript>();
                    customer.OnReceiveOrder();
                }
            }
        }
    }

    private CustomerScript FindNearestCustomer()
    {
        CustomerScript current = null;
        foreach (GameObject customer in CustomerSpawnManager.Instance.customerPool.objects)
        {
            if (customer.GetComponent<CustomerScript>().IsAtCounterSpot) 
            { 
                if(current == null || Vector3.Distance(customer.gameObject.transform.position, transform.position) < Vector3.Distance(current.gameObject.transform.position, gameObject.transform.position))
                {
                    current = customer.GetComponent<CustomerScript>();
                }
            }
        }
        return current;
    }
}
