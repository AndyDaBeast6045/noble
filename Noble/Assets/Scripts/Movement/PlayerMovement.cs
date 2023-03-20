using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Serialized Fields used by this script
    [SerializeField]
    GameObject player;

    //[SerializeField]
    // Camera Camera;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float dashForce;

    [SerializeField]
    float playerSpeed;

    [SerializeField]
    bool isGrounded;

    [SerializeField]
    bool isFlipped;

    [SerializeField]
    Vector2 spawnLocation;

    private float horizontal;
    private bool canJump;
    private bool canDash;
    private int jumpCounter;
    private float coyoteTime;
    private float coyoteTimeDelta;
    private float dashTime;
    private float dashTimeDelta;

    // start method
    private void Start()
    {
        dashTime = 2f;
        dashTimeDelta = dashTime;
        jumpCounter = 0;
        jumpForce = 275f;
        playerSpeed = 3f;
        isGrounded = true;
        isFlipped = false;
        spawnLocation = player.transform.position;
    }

    // update function
    private void Update()
    {
        // get the direction that the player is facing
        horizontal = Input.GetAxisRaw("Horizontal");

        // sets the status if the player can jump or not
        if (Input.GetKeyDown(KeyCode.X) && jumpCounter < 2)
        {
            canJump = true;
        }

        // half the y velocity of the player and set can jump to false
        if (Input.GetKeyUp(KeyCode.X))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            canJump = false;
        }

        // start dashing
        if (Input.GetKeyDown(KeyCode.Z) && jumpCounter == 0)
        {
            canDash = true;
        }

        // decrement dash time
        if (canDash)
            dashTimeDelta -= Time.deltaTime;

        // reset dash after dash time is less than or equal to 0
        if (dashTimeDelta <= 0 && isGrounded)
        {
            canDash = false;
            dashTimeDelta = dashTime;
        }

        // Stop dashing, when not holding down the dash button
        if ((!Input.GetKey(KeyCode.Z) && isGrounded))
        {
            canDash = false;
        }
    }

    // fixed update, physics operations done here
    private void FixedUpdate()
    {
        // set the direction the player is facing
        if (!isFlipped && horizontal == -1)
        {
            player.transform.Rotate(new Vector3(0, 180, 0));
            isFlipped = true;
        }

        if (isFlipped && horizontal == 1)
        {
            player.transform.Rotate(new Vector3(0, -180, 0));
            isFlipped = false;
        }

        // get the player speed;
        Vector2 speed = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        rb.velocity = speed;

        // Do jump if the player can jump
        if (canJump)
        {
            rb.gravityScale = 1;
            rb.AddForce(new Vector2(0, jumpForce));
            coyoteTimeDelta = 0;
            jumpCounter++;
            canJump = false;
        }

        // Do dash if the player is able to dash
        if (canDash)
        {
            rb.velocity = (new Vector2(horizontal * playerSpeed * 2, rb.velocity.y));
        }
    }

    // late Update
    private void LateUpdate()
    {
        if (coyoteTimeDelta > 0 && isGrounded)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }

        /*
        float offsetx = player.transform.position.x - Camera.transform.position.x;
        float offsety = (player.transform.position.y + 2f) - Camera.transform.position.y;

        if (Mathf.Abs(offsety) < 0.05f)
            offsety = 0;
        Camera.transform.Translate(new Vector2(offsetx, offsety));
        */
    }

    // check for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCounter = 0;
            isGrounded = true;
        }
    }

    // check for the end of collisions
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            isGrounded = false;
    }

    // check if entering certain triggers
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap") || collider.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = new Vector2(0, 0);
            if (isFlipped)
            {
                player.transform.Rotate(new Vector3(0, -180, 0));
                isFlipped = false;
            }
            player.transform.position = spawnLocation;
        }

        if (collider.gameObject.CompareTag("Platform"))
            isGrounded = true;
    }
}
