using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class PlayerLookScript : MonoBehaviour
{
    public GameObject head;
    public float sensitivityMin;
    public float sensitivityMax;
    public float yClampMin;
    public float yClampMax;

    private float yAngle = 0;
    private float mouseSensitivity;
    private bool isEnabled;

    private void Start()
    {
        mouseSensitivity = Prefs.GetMouseSensitivity();
    }

    void Update()
    {
        if (isEnabled)
        {
            float sensitivity = Mathf.Lerp(sensitivityMin, sensitivityMax, mouseSensitivity);
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0));
            yAngle -= Input.GetAxis("Mouse Y") * sensitivity;
            yAngle = Mathf.Clamp(yAngle, yClampMin, yClampMax);
            head.transform.localEulerAngles = new Vector3(yAngle, 0, 0);
        }
    }

    /**
     * Enable/disable whether mouse input moves the player head
     * 
     * Allows for pause menu to disable look movement while the game is pause
     * but still keep this script enabled so it can receive the CHANGED_MOUSE_SENSITIVITY
     * event if the player changes that value from the pause menu
     */
    public void SetLookEnabled(bool enabled)
    {
        isEnabled = enabled;
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
        SetLookEnabled(true);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
        SetLookEnabled(false);
    }

    private void OnChangeMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
