using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float moveSpeed = 8;
    public float jumpHeight = 2;
    public float gravity = 9.81f;
    public float airControl = 10;
    CharacterController controller;
    Vector3 input, moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver) {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
            input *= moveSpeed;
            
            if (controller.isGrounded) {
                moveDirection = input;
                if (Input.GetButton("Jump")) {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
                else {
                    moveDirection.y = 0.0f;
                }
            } else { //midair
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime * airControl);
            }

            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
