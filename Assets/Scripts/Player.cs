using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float gravity = -10f;
    public float movementSpeed = 10f;
    public CharacterController2D controller;

    private Vector3 motion; // Store the difference in movement

    private void Reset()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Horizontal Input (A / D or Left / Right arrows)
        float inputH = Input.GetAxis("Horizontal");
        // Move left / right
        motion.x = inputH * movementSpeed;
        // If the controller is on the ground
        if (controller.isGrounded)
        {
            // Reset Y
            motion.y = 0f;
        }

        // Apply gravity
        motion.y += gravity * Time.deltaTime;
        // Apply movement with motion
        controller.move(motion * Time.deltaTime);        
    }
}
