using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManagerScript : MonoBehaviour
{
    private Dictionary<FoodItemScript, Slider> foodItems;
    private Camera _camera;
    public GameObject canvas;
    public GameObject cookBarPrefab;
    public float cookBarVisibilityDistance;

    // Start is called before the first frame update
    void Start()
    {
        foodItems = new Dictionary<FoodItemScript, Slider>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<FoodItemScript, Slider> entry in foodItems) {
            FoodItemScript item = entry.Key;
            Slider slider = entry.Value;

            if (item.isCooking && Vector3.Distance(item.transform.position, _camera.transform.position) <= cookBarVisibilityDistance)
            {
                slider.gameObject.SetActive(true);
                slider.gameObject.transform.position = _camera.WorldToScreenPoint(item.transform.position + new Vector3(0, item.transform.localScale.magnitude, 0));
                slider.value = item.timeCooked / item.cookDuration;
            } else
            {
                slider.gameObject.SetActive(false);
            }
        }
    }

    public void AddItem(FoodItemScript item)
    {
        foodItems.Add(item, Instantiate(cookBarPrefab, canvas.transform).GetComponent<Slider>());
    }
}
