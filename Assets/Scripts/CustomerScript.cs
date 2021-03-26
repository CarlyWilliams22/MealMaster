using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerScript : MonoBehaviour
{
    public FoodItemScript.FoodItemType order;
    NavMeshAgent agent;
    Animator animation;
    Camera player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animator>();
        player = Camera.main;
        
    }

    private void OnEnable()
    {
        // pick a random item to order
        System.Array foodItems = FoodItemScript.FoodItemType.GetValues(typeof(FoodItemScript.FoodItemType));
        order = (FoodItemScript.FoodItemType)foodItems.GetValue(Random.Range(0, foodItems.Length));

        Messenger.Broadcast(GameEvent.CUSTOMER_CHANGE_ACTIVE, this, true);


    }

    private void OnDisable()
    {
        Messenger.Broadcast(GameEvent.CUSTOMER_CHANGE_ACTIVE, this, false);
    }

    private void Update()
    {
        if(agent.remainingDistance > 1)
        {
            animation.SetBool("isMoving", true);
        }
        else
        {
            animation.SetBool("isMoving", false);
            var lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
