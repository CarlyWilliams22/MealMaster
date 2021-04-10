using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// TODO on customer leave you will need to handle targetCustomer like targetHoldable if someone else grabs the target

public class EmployeeScript : MonoBehaviour
{
    private HoldableScript holding;
    private HoldableScript targetHoldable;
    private CustomerScript targetCustomer;
    private State state = State.FindTask;
    public GameObject grabPoint;

    private NavMeshAgent agent;

    private GameObject[] employeeCounterSpots;
    private GameObject[] foodDropOffSpots;
    private IEnumerator idleCoroutine;

    private enum State
    {
        Idle, FindTask, TakeHoldingToTable, TakeHoldingToSodaMachine, TakeHoldingToStove, TakeOrderFromCustomer, PrepareCustomerOrder, DeliverCustomerOrder
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        employeeCounterSpots = GameObject.FindGameObjectsWithTag("EmployeeCounterSpot");
        foodDropOffSpots = GameObject.FindGameObjectsWithTag("FoodDropOff");
    }

    private void OnEnable()
    {
        Messenger.AddListener<HoldableScript, bool, GameObject>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<HoldableScript, bool, GameObject>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.FindTask:
                state = State.TakeOrderFromCustomer;
                break;
            case State.TakeOrderFromCustomer:
                TakeOrderFromCustomer();
                break;
            case State.PrepareCustomerOrder:
                PrepareCustomerOrder();
                break;
            case State.DeliverCustomerOrder:
                DeliverCustomerOrder();
                break;
            default:
                break;
        }
    }

    private void Idle()
    {
        if (idleCoroutine == null)
        {
            idleCoroutine = IdleHelper();
            StartCoroutine(idleCoroutine);
        }
    }

    IEnumerator IdleHelper()
    {
        yield return new WaitForSeconds(2.0f);
        state = State.FindTask;
        idleCoroutine = null;
    }

    private void TakeOrderFromCustomer()
    {
        CustomerScript newTargetCustomer = null;
        if (!targetCustomer)
        {
            foreach (GameObject c in CustomerSpawnManager.Instance.customerPool.objects)
            {
                if (c.GetComponent<CustomerScript>().IsAtCounterSpot)
                {
                    newTargetCustomer = c.GetComponent<CustomerScript>();
                    targetCustomer = newTargetCustomer;
                    break;
                }
            }
        }

        if (newTargetCustomer)
        {
            if (agent.destination == transform.position)
            {
                agent.SetDestination(FindEmployeeCounterSpotForCustomer(newTargetCustomer));
            }
        }
        else
        {
            if (agent.destination == transform.position)
            {
                if (targetCustomer)
                {
                    CustomerUIManager.Instance.SetToughtBubble(targetCustomer, true);
                    state = State.PrepareCustomerOrder;
                }
            }
        }
    }

    private void PrepareCustomerOrder()
    {
        if (!targetHoldable)
        {
            targetHoldable = FindReadyToServeItem(targetCustomer.order);
            if (targetHoldable)
            {
                agent.SetDestination(targetHoldable.transform.position);
            }
        }
        else
        {
            if (agent.remainingDistance < 1)
            {
                targetHoldable.GetComponent<InteractableScript>().interactionEnabled = false;
                Vector3 offset = targetHoldable.GetComponent<Collider>().ClosestPoint(gameObject.transform.position);
                targetHoldable.Grab(grabPoint, offset);
                holding = targetHoldable;
                targetHoldable = null;
                agent.SetDestination(FindEmployeeCounterSpotForCustomer(targetCustomer));
                state = State.DeliverCustomerOrder;
            }
        }
    }

    private void DeliverCustomerOrder()
    {
        if (agent.remainingDistance < 1)
        {
            // transform.rotation = Quaternion.LookRotation(targetCustomer.transform.position);
            if (holding != null)
            {
                holding.Release();
                holding.transform.position = FindClosestFoodDropOff().transform.position + new Vector3(0, 0.5f, 0);
                holding = null;
                targetCustomer = null;
                state = State.Idle;
            }
        }
    }

    private Vector3 FindEmployeeCounterSpotForCustomer(CustomerScript customer)
    {
        GameObject current = employeeCounterSpots[0];
        foreach (GameObject spot in employeeCounterSpots)
        {
            if (Vector3.Distance(customer.gameObject.transform.position, spot.transform.position) < Vector3.Distance(current.transform.position, customer.gameObject.transform.position))
            {
                current = spot;
            }
        }
        return current.transform.position;
    }

    private GameObject FindClosestFoodDropOff()
    {
        GameObject current = foodDropOffSpots[0];
        foreach (GameObject spot in foodDropOffSpots)
        {
            if (Vector3.Distance(transform.position, spot.transform.position) < Vector3.Distance(current.transform.position, transform.position))
            {
                current = spot;
            }
        }
        return current;
    }


    private HoldableScript FindReadyToServeItem(FoodItemScript.FoodItemType type)
    {
        foreach (FoodItemScript item in FoodManagerScript.Instance.allFoodItems)
        {
            if (item.type == type && item.isReadyToServe && !item.GetComponent<HoldableScript>().isHeld())
            {
                return item.GetComponent<HoldableScript>();
            }
        }
        return null;
    }

    private void OnGrabHoldable(HoldableScript holdable, bool grabbed, GameObject holder)
    {
        if (grabbed)
        {
            if (targetHoldable && holdable == targetHoldable && holder != grabPoint)
            {
                targetHoldable = null;
                agent.SetDestination(agent.transform.position);
                state = State.FindTask;
            }
        }
    }
}
