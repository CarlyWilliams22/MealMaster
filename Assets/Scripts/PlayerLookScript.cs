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

    private void Start()
    {
        mouseSensitivity = Prefs.GetMouseSensitivity();
    }

    void Update()
    {
        float sensitivity = Mathf.Lerp(sensitivityMin, sensitivityMax, mouseSensitivity);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0));
        yAngle -= Input.GetAxis("Mouse Y") * sensitivity;
        yAngle = Mathf.Clamp(yAngle, yClampMin, yClampMax);
        head.transform.localEulerAngles = new Vector3(yAngle, 0, 0);
    }

    private void OnEnable()
    {
        Messenger.AddListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<float>(GameEvent.CHANGED_MOUSE_SENSITIVITY, OnChangeMouseSensitivity);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnChangeMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
