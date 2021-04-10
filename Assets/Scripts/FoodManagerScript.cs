using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManagerScript : MonoBehaviour
{
    public HashSet<FoodItemScript> allFoodItems;
    private Dictionary<FoodItemScript, Slider> uiFoodItems;
    private Dictionary<InventoryItemScript, Text> inventoryItems;
    private Camera _camera;
    public GameObject canvas;
    public GameObject cookBarPrefab;
    public GameObject inventoryItemTextPrefab;
    public float uiVisibilityDistance;

    private static FoodManagerScript _instance = null;
    public static FoodManagerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiFoodItems = new Dictionary<FoodItemScript, Slider>();
        inventoryItems = new Dictionary<InventoryItemScript, Text>();
        allFoodItems = new HashSet<FoodItemScript>();
        _camera = Camera.main;

        InventoryItemScript[] allInventoryItems = FindObjectsOfType<InventoryItemScript>();
        foreach (InventoryItemScript item in allInventoryItems)
        {
            inventoryItems.Add(item, Instantiate(inventoryItemTextPrefab, canvas.transform).GetComponent<Text>());
        }

        FoodItemScript[] startingFoodItems = FindObjectsOfType<FoodItemScript>();
        foreach (FoodItemScript item in startingFoodItems)
        {
            AddItem(item);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<FoodItemScript, Slider> entry in uiFoodItems) {
            FoodItemScript item = entry.Key;
            Slider slider = entry.Value;

            if (item.isCooking && IsInFrontOfCamera(item.gameObject) && IsWithinVisibility(item.gameObject))
            {
                slider.gameObject.SetActive(true);
                slider.gameObject.transform.position = _camera.WorldToScreenPoint(item.transform.position + new Vector3(0, item.transform.localScale.magnitude, 0));
                slider.value = item.timeCooked / item.cookDuration;
            } else
            {
                slider.gameObject.SetActive(false);
            }
        }

        foreach (KeyValuePair<InventoryItemScript, Text> entry in inventoryItems)
        {
            InventoryItemScript item = entry.Key;
            Text label = entry.Value;
            if (IsInFrontOfCamera(item.gameObject) && IsWithinVisibility(item.gameObject)) {
                label.gameObject.SetActive(true);
                label.gameObject.transform.position = _camera.WorldToScreenPoint(item.transform.position);
                label.text = item.count.ToString();
                label.color = item.count > 0 ? Color.green : Color.red;
            }
            else
            {
                label.gameObject.SetActive(false);
            }
        }
    }

    public void AddUIItem(FoodItemScript item)
    {
        uiFoodItems.Add(item, Instantiate(cookBarPrefab, canvas.transform).GetComponent<Slider>());
    }

    public void AddItem(FoodItemScript item)
    {
        allFoodItems.Add(item);
    }

    public void RemoveItem(FoodItemScript item)
    {
        allFoodItems.Remove(item);

        Slider slider;
        if (uiFoodItems.TryGetValue(item, out slider))
        {
            Destroy(slider);
        }
    }

    private bool IsInFrontOfCamera(GameObject obj)
    {
        return Vector3.Dot((obj.transform.position - _camera.transform.position).normalized, _camera.transform.forward) > 0;
    }

    private bool IsWithinVisibility(GameObject obj)
    {
        return Vector3.Distance(obj.transform.position, _camera.transform.position) <= uiVisibilityDistance;
    }
}
