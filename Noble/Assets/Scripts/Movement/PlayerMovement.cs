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

    private Vector2 speed;
    private float horizontal;
    private bool canJump;
    private bool canDash;
    private bool inputIsEnabled;
    private bool isFacingAndTouchingWall;
    private int jumpCounter;
    private float coyoteTime;
    private float coyoteTimeDelta;
    private float dashTime;
    private float dashTimeDelta;

    // start method
    private void Start()
    {
        dashTime = 0.6f; // should last for approximately 60 frames
        dashTimeDelta = dashTime;
        jumpCounter = 0;
        jumpForce = 275f;
        playerSpeed = 3f;
        inputIsEnabled = true;
        isGrounded = true;
        isFlipped = false;
        isFacingAndTouchingWall = false;
        spawnLocation = player.transform.position;
    }

    // update function
    private void Update()
    {
        // check and see if the player is on the lowest platform
        if (player.transform.position.y <= 1.25f)
            isGrounded = true;

        // Check and see if the player can take in input

        if (isGrounded)
        {
            inputIsEnabled = true;
            isFacingAndTouchingWall = false;
        }

        // get the direction that the player is facing
        horizontal = Input.GetAxisRaw("Horizontal");

        // sets the status if the player can jump or not
        if (Input.GetKeyDown(KeyCode.X) && jumpCounter < 1)
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
        if (Input.GetKeyDown(KeyCode.Z) && jumpCounter == 0 && inputIsEnabled)
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
        Debug.Log("here is if the player is facing the wall" + isFacingAndTouchingWall);
        // set the direction the player is facing
        if (!isFlipped && horizontal == -1)
        {
            player.transform.Rotate(new Vector3(0, 180, 0));
            isFlipped = true;
            if (!inputIsEnabled)
                isFacingAndTouchingWall = !isFacingAndTouchingWall;
        }

        if (isFlipped && horizontal == 1)
        {
            player.transform.Rotate(new Vector3(0, -180, 0));
            isFlipped = false;
            if (!inputIsEnabled)
                isFacingAndTouchingWall = !isFacingAndTouchingWall;
        }

        // get the player speed;

        if (inputIsEnabled || !isFacingAndTouchingWall)
        {
            speed = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        }
        else
        {
            speed = new Vector2(0, rb.velocity.y);
        }

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
    }

    // check for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isFacingAndTouchingWall = true;
            canDash = false;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCounter = 0;
            if (inputIsEnabled)
                isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !isGrounded)
        {
            inputIsEnabled = false;
        }
    }

    // check for the end of collisions
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            isGrounded = false;
        else if (collision.gameObject.CompareTag("Wall"))
        {
            inputIsEnabled = true;
            isFacingAndTouchingWall = false;
        }
    }

    // check if entering certain triggers
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Wall") && !isGrounded)
        {
            inputIsEnabled = false;
            speed = new Vector2(0, rb.velocity.y);
        }
        else if (collider.gameObject.CompareTag("Trap") || collider.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = new Vector2(0, 0);
            if (isFlipped)
            {
                player.transform.Rotate(new Vector3(0, -180, 0));
                isFlipped = false;
            }
            player.transform.position = spawnLocation;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Did I leave the wall?");
            inputIsEnabled = true;
        }
    }
}
