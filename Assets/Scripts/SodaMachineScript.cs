using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodaMachineScript : MonoBehaviour
{
    public ParticleSystem spout1Effect;
    public ParticleSystem spout2Effect;

    private HashSet<GameObject> fillArea1Drinks;
    private HashSet<GameObject> fillArea2Drinks;

    private void Start()
    {
        fillArea1Drinks = new HashSet<GameObject>();
        fillArea2Drinks = new HashSet<GameObject>();
    }

    public void OnDrinkEnterFillArea(GameObject fillArea, GameObject drink)
    {
        switch (fillArea.tag)
        {
            case "SodaMachineFillArea1":
                fillArea1Drinks.Add(drink);
                break;
            case "SodaMachineFillArea2":
                fillArea2Drinks.Add(drink);
                break;
        }
        UpdateEffects();
    }

    public void OnDrinkExitFillArea(GameObject fillArea, GameObject drink)
    {
        switch (fillArea.tag)
        {
            case "SodaMachineFillArea1":
                fillArea1Drinks.Remove(drink);
                break;
            case "SodaMachineFillArea2":
                fillArea2Drinks.Remove(drink);
                break;
        }
        UpdateEffects();
    }

    private void UpdateEffects()
    {
        if (fillArea1Drinks.Count > 0) {
            spout1Effect.Play();
        } 
        else
        {
            spout1Effect.Stop();
        }

        if (fillArea2Drinks.Count > 0)
        {
            spout2Effect.Play();
        }
        else
        {
            spout2Effect.Stop();
        }
    }
}
