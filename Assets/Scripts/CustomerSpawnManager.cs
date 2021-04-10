using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawnManager : MonoBehaviour
{

    public GameObject customer1, customer2, customer3, point1, point2, point3;
    public ObjectPool customerPool;
    GameObject newCustomer;
    public GameObject[] spots = new GameObject[3];
    bool readyToSpawn = true;
    public GameObject customerDespawnPosition;

    private static CustomerSpawnManager _instance = null;

    public static CustomerSpawnManager Instance
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
        GameObject[] prefabs = { customer1, customer2, customer3};
        customerPool = new ObjectPool(prefabs, 3, false);
        spots[0] = point1;
        spots[1] = point2;
        spots[2] = point3;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToSpawn)
        {
            newCustomer = customerPool.GetObject();
            if (newCustomer)
            {
                for (int i = 0; i < spots.Length; i++)
                {
                    if (spots[i].GetComponent<SpotScript>().isOpen)
                    {
                        newCustomer.GetComponent<NavMeshAgent>().Warp(transform.position);
                        newCustomer.GetComponent<NavMeshAgent>().SetDestination(spots[i].transform.position);
                        spots[i].GetComponent<SpotScript>().occupier = newCustomer.GetComponent<CustomerScript>();
                        newCustomer.GetComponent<CustomerScript>().spot = spots[i].GetComponent<SpotScript>();
                        break;
                    }
                }
                readyToSpawn = false;
                Invoke("nextCustomer", 3);
            }
        }
    }

    void nextCustomer()
    {
        readyToSpawn = true;
    }
}
