using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts;

public class CustomerSpawnManager : MonoBehaviour
{

    public GameObject customer1, customer2, customer3, point1, point2, point3;
    public ObjectPool customerPool;
    GameObject newCustomer;
    public GameObject[] spots = new GameObject[3];
    public AudioClip enterBell, moneySound;
    bool readyToSpawn = true;
    public GameObject customerDespawnPosition;
    AudioSource audioPlayer;

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
        audioPlayer = GetComponent<AudioSource>();
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
                Invoke("EnterDiner", 2);
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
                Invoke("nextCustomer", 28 - 25*Prefs.GetPopularityScore());
            }
        }
    }

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.MONEY_RECIVED, MoneySound);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.MONEY_RECIVED, MoneySound);
    }

    void nextCustomer()
    {
        readyToSpawn = true;
    }

    void EnterDiner()
    {
        audioPlayer.PlayOneShot(enterBell);
    }

    void MoneySound()
    {
        audioPlayer.PlayOneShot(moneySound);
    }
}
