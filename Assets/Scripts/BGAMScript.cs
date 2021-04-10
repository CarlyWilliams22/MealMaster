using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAMScript : MonoBehaviour
{

    AudioSource audioPlayer;

    public AudioClip alarm, bgMusic;

    private HashSet<BurnableFoodItemScript> itemsOnFire;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        itemsOnFire = new HashSet<BurnableFoodItemScript>();
    }


    private void OnEnable()
    {
        Messenger.AddListener<BurnableFoodItemScript>(GameEvent.ON_FIRE, fireAlarmStart);
        Messenger.AddListener<BurnableFoodItemScript>(GameEvent.OFF_FIRE, fireAlarmStop);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<BurnableFoodItemScript>(GameEvent.ON_FIRE, fireAlarmStart);
        Messenger.RemoveListener<BurnableFoodItemScript>(GameEvent.OFF_FIRE, fireAlarmStop);
    }

    public void fireAlarmStart(BurnableFoodItemScript item)
    {
        itemsOnFire.Add(item);
        if (itemsOnFire.Count > 0)
        {
            audioPlayer.Stop();
            audioPlayer.clip = alarm;
            audioPlayer.Play();
        }
    }

    public void fireAlarmStop(BurnableFoodItemScript item)
    {
        itemsOnFire.Remove(item);
        if (itemsOnFire.Count == 0)
        {
            audioPlayer.Stop();
            audioPlayer.clip = bgMusic;
            audioPlayer.Play();
        }

    }
}
