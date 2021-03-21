using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public float speed;
    public float gravity;

    private CharacterController controller;
    private float ySpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            ySpeed = -1;
        } else
        {
            ySpeed += Time.deltaTime * gravity;
        }

        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVec = transform.rotation * Vector3.ClampMagnitude(moveInput * speed, speed);
        moveVec.y += ySpeed;
        controller.Move(moveVec * Time.deltaTime);
    }
}
