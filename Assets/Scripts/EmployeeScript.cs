using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

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

    private GameObject[] employeeSodaMachineSpots;
    private GameObject[] employeeStoveSpots;
    private GameObject[] sodaMachineSodaFillSpots;

    private GameObject sodaFillSpot;

    private InventoryItemScript inventoryDrink;
    private GameObject inventoryRoomEmployeeSpot;
    

    private enum State
    {
        Idle, 
        FindTask,
        TakeHoldingToTable, 
        TakeHoldingToSodaMachine, 
        TakeHoldingToStove,
        TakeOrderFromCustomer, 
        GrabReadyToServeItem, 
        DeliverHoldingToCustomer, 
        GetRawDrink, // find an existing uncooked drink
        GetRawBurger,
        GetRawBun,
        GetInventoryDrink, // get a new drink
        GetInventoryBurger,
        GetInventoryBun
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        employeeCounterSpots = GameObject.FindGameObjectsWithTag("EmployeeCounterSpot");
        foodDropOffSpots = GameObject.FindGameObjectsWithTag("FoodDropOff");
        employeeSodaMachineSpots = GameObject.FindGameObjectsWithTag("EmployeeSodaMachineSpot"); ;
        sodaMachineSodaFillSpots = GameObject.FindGameObjectsWithTag("SodaFillSpot");
        inventoryDrink = GameObject.FindGameObjectWithTag("InventoryCup").GetComponent<InventoryItemScript>();
        inventoryRoomEmployeeSpot = GameObject.FindGameObjectWithTag("InventoryRoomEmployeeSpot");
    }

    private void OnEnable()
    {
        Messenger.AddListener<HoldableScript, bool, GameObject>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
        Messenger.AddListener<CustomerScript>(GameEvent.CUSTOMER_LEAVE_SPOT, OnCustomerLeaveSpot);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<HoldableScript, bool, GameObject>(GameEvent.GRAB_HOLDABLE, OnGrabHoldable);
        Messenger.RemoveListener<CustomerScript>(GameEvent.CUSTOMER_LEAVE_SPOT, OnCustomerLeaveSpot);
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
                FindTask();
                break;
            case State.TakeOrderFromCustomer:
                TakeOrderFromCustomer();
                break;
            case State.GrabReadyToServeItem:
                GrabReadyToServeItem();
                break;
            case State.DeliverHoldingToCustomer:
                DeliverHoldingToCustomer();
                break;
            case State.GetRawDrink:
                GetRawDrink();
                break;
            case State.GetInventoryDrink:
                GetInventoryDrink();
                break;
            case State.TakeHoldingToSodaMachine:
                TakeHoldingToSodaMachine();
                break;
            default:
                break;
        }
    }

    private void FindTask()
    {
        if (!targetCustomer)
        {
            state = State.TakeOrderFromCustomer;
        } 
        else
        {
            state = State.GrabReadyToServeItem;
        }
    }

    private void Idle()
    {
        if (idleCoroutine == null)
        {
            targetCustomer = null;
            targetHoldable = null;
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
                    CustomerUIManager.Instance.SetThoughtBubble(targetCustomer, true);
                    state = State.GrabReadyToServeItem;
                }
            }
        }
    }

    private void GrabReadyToServeItem()
    {
        if (!targetHoldable)
        {
            targetHoldable = FindReadyToServeItem(targetCustomer.order);
            if (targetHoldable)
            {
                agent.SetDestination(targetHoldable.transform.position);
            } else
            {
                switch (targetCustomer.order)
                {
                    case FoodItemScript.FoodItemType.DRINK:
                        state = State.GetRawDrink;
                        break;
                    // TODO handle burgers
                    default:
                        state = State.FindTask;
                        break;
                }
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
                state = State.DeliverHoldingToCustomer;
            }
        }
    }

    private void DeliverHoldingToCustomer()
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
            }
            state = State.Idle;
        }
    }

    private void GetRawDrink()
    {
        if (!targetHoldable)
        {
            targetHoldable = FindRawItem(FoodItemScript.FoodItemType.DRINK);
            if (targetHoldable)
            {
                agent.SetDestination(targetHoldable.transform.position);
            } else
            {
                state = State.GetInventoryDrink;
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

                InitTakeHoldingToSodaMachine();
            }
        }
    }

    private void GetInventoryDrink()
    {
        /**
         * agent.SetDestination doesn't update agent.remainingDistance on the same frame,
         * so manually check Vector3.Distance too
         */
        agent.SetDestination(inventoryRoomEmployeeSpot.transform.position);
        if (agent.remainingDistance < 1 && Vector3.Distance(inventoryRoomEmployeeSpot.transform.position, transform.position) < 1)
        {
            GameObject item = inventoryDrink.InstantiateItem();
            if (item)
            {
                holding = item.GetComponent<HoldableScript>();
                Vector3 offset = holding.GetComponent<Collider>().ClosestPoint(gameObject.transform.position);
                holding.GetComponent<InteractableScript>().interactionEnabled = false;
                holding.Grab(grabPoint, offset);
                InitTakeHoldingToSodaMachine();
            } 
            else
            {
                state = State.FindTask;
            }
        }
    }

    private void InitTakeHoldingToSodaMachine()
    {
        sodaFillSpot = FindEmptySodaMachineDrinkFillSpot();
        if (sodaFillSpot)
        {
            Vector3 employeeSpot = SodaMachineEployeeSpotClosestToPoint(sodaFillSpot.transform.position);
            agent.SetDestination(employeeSpot);
            state = State.TakeHoldingToSodaMachine;
        }
        else
        {
            state = State.TakeHoldingToTable;
        }
    }

    private void TakeHoldingToSodaMachine()
    {
        if (agent.remainingDistance < 1)
        {
            if (holding && sodaFillSpot)
            {
                holding.Release();
                holding.transform.position = sodaFillSpot.gameObject.transform.position;
                holding.GetComponent<InteractableScript>().interactionEnabled = true;
                holding = null;
            }
            state = State.FindTask;
        }
    }

    private Vector3 SodaMachineEployeeSpotClosestToPoint(Vector3 point)
    {
        GameObject current = employeeSodaMachineSpots[0];
        foreach (GameObject spot in employeeSodaMachineSpots)
        {
            if (Vector3.Distance(point, spot.transform.position) < Vector3.Distance(point, current.transform.position)) {
                current = spot;
            }
        }
        return current.transform.position;
    }

    private GameObject FindEmptySodaMachineDrinkFillSpot()
    {
        foreach (GameObject spot in sodaMachineSodaFillSpots)
        {
            if (FoodManagerScript.Instance.allFoodItems.All(item => item.type != FoodItemScript.FoodItemType.DRINK || Vector3.Distance(spot.gameObject.transform.position, item.gameObject.transform.position) > 0.5f))
            {
                return spot;
            }
        }
        return null;
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

    private HoldableScript FindRawItem(FoodItemScript.FoodItemType type)
    {
        foreach (FoodItemScript item in FoodManagerScript.Instance.allFoodItems)
        {
            if (item.type == type && !item.isSpoiled && !item.isReadyToServe && !item.isCooking && !item.GetComponent<HoldableScript>().isHeld())
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

    private void OnCustomerLeaveSpot(CustomerScript customer)
    {
        if (customer == targetCustomer)
        {
            targetHoldable = null;
            targetCustomer = null;
            if (holding)
            {
                state = State.TakeHoldingToTable;
            } else
            {
                state = State.FindTask;
            }
        }
    }
}
