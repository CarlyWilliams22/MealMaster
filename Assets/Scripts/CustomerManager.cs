using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public GameObject canvas;
    public float uiVisibilityDistance;
    public GameObject burgerThoughtPrefab;
    public GameObject drinkThoughtPrefab;

    private Dictionary<CustomerScript, OrderUI> customers;
    private Camera _camera;

    private struct OrderUI
    {
        public GameObject thoughtBubble;
        public bool isOpen;

        public OrderUI(GameObject thoughtBubble, bool isOpen)
        {
            this.thoughtBubble = thoughtBubble;
            this.isOpen = isOpen;
        }
    }

    private void Awake()
    {
        customers = new Dictionary<CustomerScript, OrderUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<CustomerScript, OrderUI> entry in customers)
        {
            CustomerScript customer = entry.Key;
            OrderUI orderUI = entry.Value;

            if (orderUI.isOpen && IsInFrontOfCamera(customer.gameObject) && IsWithinVisibility(customer.gameObject))
            {
                orderUI.thoughtBubble.gameObject.SetActive(true);
                orderUI.thoughtBubble.transform.position = _camera.WorldToScreenPoint(customer.transform.position + new Vector3(0, customer.gameObject.transform.localScale.magnitude, 0));
            }
            else
            {
                orderUI.thoughtBubble.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable() { 
        Messenger.AddListener<InteractableScript>(GameEvent.CLICK_INTERACTABLE, OnClickInteractable);
        Messenger.AddListener<CustomerScript, bool>(GameEvent.CUSTOMER_CHANGE_ACTIVE, OnCustomerChangeActive);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<InteractableScript>(GameEvent.CLICK_INTERACTABLE, OnClickInteractable);
        Messenger.RemoveListener<CustomerScript, bool>(GameEvent.CUSTOMER_CHANGE_ACTIVE, OnCustomerChangeActive);
    }

    private GameObject CreateThoughtBubble(CustomerScript customer)
    {
        GameObject prefab;
        switch (customer.order)
        {
            case FoodItemScript.FoodItemType.BURGER:
                prefab = burgerThoughtPrefab;
                break;
            case FoodItemScript.FoodItemType.DRINK:
                prefab = drinkThoughtPrefab;
                break;
            default:
                prefab = null;
                break;
        }
        return prefab ? Instantiate(prefab, canvas.transform) : null;
    }

    private bool IsInFrontOfCamera(GameObject obj)
    {
        return Vector3.Dot((obj.transform.position - _camera.transform.position).normalized, _camera.transform.forward) > 0;
    }

    private bool IsWithinVisibility(GameObject obj)
    {
        return Vector3.Distance(obj.transform.position, _camera.transform.position) <= uiVisibilityDistance;
    }

    private void OnClickInteractable(InteractableScript interactable)
    {
        if (interactable.gameObject.CompareTag("Customer"))
        {
            CustomerScript customer = interactable.GetComponent<CustomerScript>();
            OrderUI orderUI;
            if (customers.TryGetValue(customer, out orderUI))
            {
                customers[customer] = new OrderUI(orderUI.thoughtBubble, !orderUI.isOpen);
            }
        }
    }

    private void OnCustomerChangeActive(CustomerScript customer, bool active)
    {
        if (active)
        {
            if (customers.ContainsKey(customer))
            {
                Destroy(customers[customer].thoughtBubble);
            }
            customers[customer] = new OrderUI(CreateThoughtBubble(customer), false);
        } else
        {
            if (customers.ContainsKey(customer))
            {
                Destroy(customers[customer].thoughtBubble);
                customers.Remove(customer);
            }
        }
    }
}
