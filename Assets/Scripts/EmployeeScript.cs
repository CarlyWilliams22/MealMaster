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
    private new Animator animation;

    private GameObject[] employeeCounterSpots;
    private GameObject[] foodDropOffSpots;
    private IEnumerator idleCoroutine;

    private GameObject[] employeeSodaMachineSpots;
    private GameObject[] employeeStoveSpots;
    private GameObject[] sodaMachineSodaFillSpots;

    private GameObject sodaFillSpot;
    private GameObject stoveDropSpot;

    private InventoryItemScript inventoryDrink;
    private InventoryItemScript inventoryBurger;
    private InventoryItemScript inventoryBun;
    private GameObject inventoryRoomEmployeeSpot;

    private GameObject employeeTableSpot;
    private GameObject tableDropSpot;
    private GameObject[] stoveDropSpots;


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
        GetInventoryDrink, // get a new drink
        GetInventoryBurger,
        GetInventoryBun,
        GetCookedBurger
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animator>();
        employeeCounterSpots = GameObject.FindGameObjectsWithTag("EmployeeCounterSpot");
        foodDropOffSpots = GameObject.FindGameObjectsWithTag("FoodDropOff");
        employeeSodaMachineSpots = GameObject.FindGameObjectsWithTag("EmployeeSodaMachineSpot"); ;
        sodaMachineSodaFillSpots = GameObject.FindGameObjectsWithTag("SodaFillSpot");
        inventoryDrink = GameObject.FindGameObjectWithTag("InventoryCup").GetComponent<InventoryItemScript>();
        inventoryBurger = GameObject.FindGameObjectWithTag("InventoryBurger").GetComponent<InventoryItemScript>();
        inventoryBun = GameObject.FindGameObjectWithTag("InventoryBun").GetComponent<InventoryItemScript>();
        inventoryRoomEmployeeSpot = GameObject.FindGameObjectWithTag("InventoryRoomEmployeeSpot");
        employeeTableSpot = GameObject.FindGameObjectWithTag("EmployeeTableSpot");
        tableDropSpot = GameObject.FindGameObjectWithTag("TableDropSpot");
        stoveDropSpots = GameObject.FindGameObjectsWithTag("StoveDropSpot");
        employeeStoveSpots = GameObject.FindGameObjectsWithTag("EmployeeStoveSpot");
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

    void UpdateAnimation()
    {
        if (agent.remainingDistance > 0.5)
        {
            animation.SetBool("isMoving", true);
        }
        else
        {
            animation.SetBool("isMoving", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
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
            case State.TakeHoldingToTable:
                TakeHoldingToTable();
                break;
            case State.GetCookedBurger:
                GetCookedBurger();
                break;
            case State.GetRawBurger:
                GetRawBurger();
                break;
            case State.GetInventoryBurger:
                GetInventoryBurger();
                break;
            case State.TakeHoldingToStove:
                TakeHoldingToStove();
                break;
            case State.GetInventoryBun:
                GetInventoryBun();
                break;
            default:
                break;
        }
    }

    private void TransitionTo(State s)
    {
        switch (s)
        {
            case State.TakeHoldingToSodaMachine:
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
                    TransitionTo(State.TakeHoldingToTable);
                }
                break;
            }
            case State.TakeHoldingToStove:
            {
                stoveDropSpot = stoveDropSpots[Random.Range(0, stoveDropSpots.Length)];
                Vector3 employeeSpot = StoveEployeeSpotClosestToPoint(stoveDropSpot.transform.position);
                agent.SetDestination(employeeSpot);
                state = State.TakeHoldingToStove;
                break;
            }
            case State.TakeHoldingToTable:
                agent.SetDestination(employeeTableSpot.transform.position);
                state = State.TakeHoldingToTable;
                break;
            case State.GetRawDrink:
                if (!FindEmptySodaMachineDrinkFillSpot())
                {
                    state = State.GetInventoryDrink;
                } else
                {
                    state = State.GetRawDrink;
                }
                break;
            default:
                state = s;
                break;
        }
    }

    private void FindTask()
    {
        if (!targetCustomer)
        {
            TransitionTo(State.TakeOrderFromCustomer);
        } 
        else
        {
            TransitionTo(State.GrabReadyToServeItem);
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
        TransitionTo(State.FindTask);
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
                    TransitionTo(State.GrabReadyToServeItem);
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
                        TransitionTo(State.GetRawDrink);
                        break;
                    case FoodItemScript.FoodItemType.BURGER:
                        TransitionTo(State.GetCookedBurger);
                        break;
                    default:
                        TransitionTo(State.FindTask);
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
                TransitionTo(State.DeliverHoldingToCustomer);
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
            TransitionTo(State.Idle);
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
                TransitionTo(State.GetInventoryDrink);
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
                TransitionTo(State.TakeHoldingToSodaMachine);
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
                TransitionTo(State.TakeHoldingToSodaMachine);
            } 
            else
            {
                TransitionTo(State.FindTask);
            }
        }
    }

    private void TakeHoldingToSodaMachine()
    {
        if (agent.remainingDistance < 1)
        {
            if (holding)
            {
                holding.Release();
                holding.transform.position = sodaFillSpot.gameObject.transform.position;
                holding.GetComponent<InteractableScript>().interactionEnabled = true;
                holding = null;
            }
            TransitionTo(State.FindTask);
        }
    }

    private void TakeHoldingToTable()
    {
        if (agent.remainingDistance < 1)
        {
            if (holding)
            {
                holding.Release();
                holding.transform.position = new Vector3(tableDropSpot.transform.position.x + Random.Range(-0.5f, 0.5f), tableDropSpot.transform.position.y, tableDropSpot.transform.position.z + Random.Range(-0.5f, 0.5f));
                holding.GetComponent<InteractableScript>().interactionEnabled = true;
                holding = null;
            }
            TransitionTo(State.FindTask);
        }
    }

    private void GetCookedBurger()
    {
        if (!targetHoldable)
        {
            targetHoldable = FindCookedBurger();
            if (targetHoldable)
            {
                agent.SetDestination(targetHoldable.transform.position);
            }
            else
            {
                TransitionTo(State.GetRawBurger);
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
                TransitionTo(State.TakeHoldingToTable);
            }
        }
    }

    private void GetRawBurger()
    {
        if (!targetHoldable)
        {
            targetHoldable = FindRawItem(FoodItemScript.FoodItemType.BURGER);
            if (targetHoldable)
            {
                agent.SetDestination(targetHoldable.transform.position);
            }
            else
            {
                TransitionTo(State.GetInventoryBurger);
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
                TransitionTo(State.TakeHoldingToStove);
            }
        }
    }

    private void GetInventoryBurger()
    {
        agent.SetDestination(inventoryRoomEmployeeSpot.transform.position);
        if (agent.remainingDistance < 1 && Vector3.Distance(inventoryRoomEmployeeSpot.transform.position, transform.position) < 1)
        {
            GameObject item = inventoryBurger.InstantiateItem();
            if (item)
            {
                holding = item.GetComponent<HoldableScript>();
                Vector3 offset = holding.GetComponent<Collider>().ClosestPoint(gameObject.transform.position);
                holding.GetComponent<InteractableScript>().interactionEnabled = false;
                holding.Grab(grabPoint, offset);
                TransitionTo(State.TakeHoldingToStove);
            }
            else
            {
                TransitionTo(State.FindTask);
            }
        }
    }

    private void GetInventoryBun()
    {
        agent.SetDestination(inventoryRoomEmployeeSpot.transform.position);
        if (agent.remainingDistance < 1 && Vector3.Distance(inventoryRoomEmployeeSpot.transform.position, transform.position) < 1)
        {
            GameObject item = inventoryBun.InstantiateItem();
            if (item)
            {
                holding = item.GetComponent<HoldableScript>();
                Vector3 offset = holding.GetComponent<Collider>().ClosestPoint(gameObject.transform.position);
                holding.GetComponent<InteractableScript>().interactionEnabled = false;
                holding.Grab(grabPoint, offset);
                TransitionTo(State.TakeHoldingToTable);
            }
            else
            {
                TransitionTo(State.FindTask);
            }
        }
    }

    private void TakeHoldingToStove()
    {
        if (agent.remainingDistance < 1)
        {
            if (holding)
            {
                holding.Release();
                holding.transform.position = new Vector3(stoveDropSpot.transform.position.x + Random.Range(-0.5f, 0.5f), stoveDropSpot.transform.position.y, stoveDropSpot.transform.position.z + Random.Range(-0.5f, 0.5f));
                holding.GetComponent<InteractableScript>().interactionEnabled = true;
                holding = null;
            }
            TransitionTo(State.GetInventoryBun);
        }
    }

    private HoldableScript FindCookedBurger()
    {
        foreach (FoodItemScript item in FoodManagerScript.Instance.allFoodItems)
        {
            if (item.type == FoodItemScript.FoodItemType.BURGER && item.isCooked && !item.isSpoiled && !item.isReadyToServe &&!item.GetComponent<HoldableScript>().isHeld())
            {
                return item.GetComponent<HoldableScript>();
            }
        }
        return null;
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

    private Vector3 StoveEployeeSpotClosestToPoint(Vector3 point)
    {
        GameObject current = employeeStoveSpots[0];
        foreach (GameObject spot in employeeStoveSpots)
        {
            if (Vector3.Distance(point, spot.transform.position) < Vector3.Distance(point, current.transform.position))
            {
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
                print(item.gameObject.name);
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
                TransitionTo(State.FindTask);
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
                TransitionTo(State.TakeHoldingToTable);
            } else
            {
                TransitionTo(State.FindTask);
            }
        }
    }
}
