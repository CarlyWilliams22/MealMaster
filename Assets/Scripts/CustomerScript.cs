using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CustomerScript : MonoBehaviour
{
    public FoodItemScript.FoodItemType order;
    NavMeshAgent agent;
    new Animator animation;
    Camera player;
    bool pastFirstUpdate;
    public GameObject grabPoint;
    public HoldableScript holding;
    bool leaving;
    public SpotScript spot;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animator>();
    }

    private void Start()
    {
        player = Camera.main;
        
    }

    private void OnEnable()
    {
        // pick a random item to order
        System.Array foodItems = FoodItemScript.FoodItemType.GetValues(typeof(FoodItemScript.FoodItemType));
        order = (FoodItemScript.FoodItemType)foodItems.GetValue(Random.Range(0, foodItems.Length-1));
        leaving = false;
        pastFirstUpdate = false;
        CustomerUIManager.Instance.SetToughtBubble(this, false);
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
        pastFirstUpdate = true;

        if (leaving && agent.remainingDistance < 1)
        {
            Despawn();
        }
    }

    public bool IsAtCounterSpot
    {
        get => gameObject.activeSelf && pastFirstUpdate && !leaving && agent.remainingDistance < 1 && CustomerSpawnManager.Instance.spots.Any(s => s.GetComponent<SpotScript>().occupier == this);
    }

    public void OnReceiveOrder()
    {
        leaving = true;
        agent.SetDestination(CustomerSpawnManager.Instance.customerDespawnPosition.transform.position);
        CustomerUIManager.Instance.SetToughtBubble(this, false);
        Messenger.Broadcast(GameEvent.CUSTOMER_LEAVE_SPOT, this);
    }

    void Despawn()
    {
        if (holding)
        {
            Destroy(holding.gameObject);
            holding = null;
        }
        spot.occupier = null;
        gameObject.SetActive(false);
    }
}
