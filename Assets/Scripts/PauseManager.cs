using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    PlayerLookScript playerLookScript;
    private bool isPaused;

    private void Start()
    {
        playerLookScript = FindObjectOfType<PlayerLookScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            playerLookScript.enabled = !isPaused;
            Messenger.Broadcast(GameEvent.PAUSE, isPaused);
        }
    }

    private void OnEnable()
    {
        Messenger.AddListener<bool>(GameEvent.PAUSE, OnPause);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<bool>(GameEvent.PAUSE, OnPause);
    }

    private void OnPause(bool pause)
    {
        // pause/unpause game simulation time
        Time.timeScale = pause ? 0 : 1;
    }
}
