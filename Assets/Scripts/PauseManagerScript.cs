using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class PauseManagerScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public Slider mouseSensitivitySlider;

    private PlayerLookScript playerLookScript;

    private void Start()
    {
        playerLookScript = FindObjectOfType<PlayerLookScript>();
        mouseSensitivitySlider.value = Prefs.GetMouseSensitivity();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPaused(!pauseMenu.activeSelf);
        }
    }
    private void SetPaused(bool paused)
    {
        pauseMenu.SetActive(paused);
        playerLookScript.SetLookEnabled(!paused);
        Messenger.Broadcast(GameEvent.PAUSE, paused);
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

    public void OnMouseSensitivitySliderChange()
    {
        Messenger.Broadcast(GameEvent.CHANGED_MOUSE_SENSITIVITY, mouseSensitivitySlider.value);
    }

    public void OnClickResume()
    {
        SetPaused(false);
    }
}
