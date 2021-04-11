using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SodaMachineScript : MonoBehaviour
{
    public ParticleSystem spout1Effect;
    public ParticleSystem spout2Effect;
    public AudioClip pour;

    private HashSet<GameObject> fillArea1Drinks;
    private HashSet<GameObject> fillArea2Drinks;

    AudioSource audioPlayer;

    private void Start()
    {
        fillArea1Drinks = new HashSet<GameObject>();
        fillArea2Drinks = new HashSet<GameObject>();
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (spout1Effect.isPlaying || spout2Effect.isPlaying)
        {
            if (!audioPlayer.isPlaying)
            {
                audioPlayer.PlayOneShot(pour);
            }
        }
        else if( spout1Effect.isStopped && spout2Effect.isStopped)
        {
            audioPlayer.Stop();
        }
    }

    private void OnEnable()
    {
        Messenger.AddListener<FoodItemScript>(GameEvent.FOOD_ITEM_COOKED, OnFoodItemCooked);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<FoodItemScript>(GameEvent.FOOD_ITEM_COOKED, OnFoodItemCooked);
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
        if (fillArea1Drinks.Where(item => !item.GetComponent<DrinkScript>().isCooked).ToList().Count > 0) {
            spout1Effect.Play();
        } 
        else
        {
            spout1Effect.Stop();
        }

        if (fillArea2Drinks.Where(item => !item.GetComponent<DrinkScript>().isCooked).ToList().Count > 0)
        {
            spout2Effect.Play();
        }
        else
        {
            spout2Effect.Stop();
        }
    }

    private void OnFoodItemCooked(FoodItemScript item)
    {
        if (item.type == FoodItemScript.FoodItemType.DRINK)
        {
            UpdateEffects();
        }
    }
}
