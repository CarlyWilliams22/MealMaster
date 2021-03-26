using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawnManager : MonoBehaviour
{

    public GameObject customer1, customer2, customer3, point1;
    ObjectPool customerPool;
    GameObject newCustomer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] prefabs = { customer1, customer2, customer3};
        customerPool = new ObjectPool(prefabs, 1, false);
    }

    // Update is called once per frame
    void Update()
    {
        newCustomer = customerPool.GetObject();
        if (newCustomer)
        {
            newCustomer.GetComponent<NavMeshAgent>().Warp(transform.position);
            newCustomer.GetComponent<NavMeshAgent>().SetDestination(point1.transform.position);
        }
    }
}
