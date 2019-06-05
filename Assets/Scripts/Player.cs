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
    public float centreRadius = .5f;

    public CharacterController2D controller;
    public Animator anim;
    public SpriteRenderer rend;

    private Vector3 velocity; // Store the difference in movement
    private bool isClimbing = false;

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
        float inputV = Input.GetAxis("Vertical");

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // If the controller is on the ground
        if (!controller.isGrounded && !isClimbing)
        {
            // Reset Y
            velocity.y += gravity * Time.deltaTime;
        }

        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("JumpY", velocity.y);

        // Move left or right depending on X value
        Move(inputH);
        // Climb up or down depending on Y value
        Climb(inputV, inputH);

        if (!isClimbing)
        {
            controller.move(velocity * Time.deltaTime);
        }
    }

    public void Move(float inputH)
    {
        // Move left / right
        velocity.x = inputH * movementSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        if(inputH != 0)
        {
            rend.flipX = inputH < 0;
        }
    }
    public void Climb(float inputV, float inputH)
    {
        bool isOverLadder = false; // Is the player overlapping the ladder
        Vector3 inputDir = new Vector3(inputH, inputV, 0);

        #region Part 1 - Detecting Ladders

        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);

        // Loop through all hit objects
        foreach(var hit in hits)
        {
            if (hit.tag == "Ground")
            {
                isClimbing = false;
                isOverLadder = false;
                break;
            }

            // Check if tagged "Ladder"
            if (hit.tag == "Ladder")
            {
                // Player is overlapping a Ladder!
                isOverLadder = true;
                break; // Exit just the foreach loop (works for any loop)
            }
        }

        // If the player is overlapping AND input vertical is made
        if(isOverLadder && inputV != 0)
        {
            anim.SetBool("IsClimbing", true);
            // The player is in Climbing state!
            isClimbing = true;
        }
        #endregion

        #region Part 2 - Translating the Player

        // If player is climbing
        if (isClimbing)
        {
            velocity.y = 0;
            transform.Translate(inputDir * movementSpeed * Time.deltaTime);
        }
        #endregion

        if (!isOverLadder)
        {
            anim.SetBool("IsClimbing", false);
            isClimbing = false;
        }

       // anim.SetBool("IsClimbing", isClimbing);
        anim.SetFloat("ClimbSpeed", inputDir.magnitude * movementSpeed);
    }
    public void Hurt()
    {

    }
    public void Jump()
    {
        velocity.y = jumpHeight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }
}
