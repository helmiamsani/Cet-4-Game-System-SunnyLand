using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float gravity = -10f;
    public float movementSpeed = 10f;
    public float jumpHeight = 8f;

    public CharacterController2D controller;
    public Animator anim;
    public SpriteRenderer rend;

    private Vector3 motion; // Store the difference in movement

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Horizontal Input (A / D or Left / Right arrows)
        float inputH = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Apply gravity
        motion.y += gravity * Time.deltaTime;

        // If the controller is on the ground
        if (controller.isGrounded)
        {
            // Reset Y
            motion.y = 0f;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Move left or right depending on X value
        Move(inputH);
        // Climb up or down depending on Y value
        Climb(inputY);

        // Apply movement with motion
        controller.move(motion * Time.deltaTime);
    }

    public void Move(float inputH)
    {
        // Move left / right
        motion.x = inputH * movementSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        rend.flipX = inputH < 0;
    }
    public void Climb(float inputY)
    {

    }
    public void Hurt()
    {

    }
    public void Jump()
    {
        motion.y = jumpHeight;
    }
}
