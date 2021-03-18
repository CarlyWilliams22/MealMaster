using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class PrefsManagerScript : MonoBehaviour
{
    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
    }

    private void OnChangeMouseSensitivity(float value)
    {
        Prefs.SetMouseSensitivity(value);
    }
}
