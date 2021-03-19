using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class PrefsManagerScript : MonoBehaviour
{
    private BankAccountScript bankAccount;

    private void Start()
    {
        bankAccount = FindObjectOfType<BankAccountScript>();
    }
    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
        Messenger.AddListener<bool>(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
        Messenger.RemoveListener<bool>(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
    }

    private void OnChangeMouseSensitivity(float value)
    {
        Prefs.SetMouseSensitivity(value);
    }

    private void OnLevelComplete(bool win)
    {
        if (win)
        {
            Prefs.SetCash(bankAccount.currentBalance);
        }
    }
}
