using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the input and physics related to the movement of the player
public class PlayerMovement : MonoBehaviour
{
    // game manager reference
    private EnemyRespawner respawner;

    // where the player loads in when the scene starts or when respawning
    [SerializeField]
    private Vector2 spawnLocation;

    // amount of force applied when jumping
    [SerializeField]
    private float jumpForce;

    // amount of force applied when dashing
    [SerializeField]
    private float dashForce;

    // the movement speed of the player
    [SerializeField]
    private float playerSpeed;

    // bool that reflects if the player is touching the ground
    [SerializeField]
    private bool isGrounded;

    // gets the direction the player is facing
    [SerializeField]
    private bool isFlipped;

    // vars that are initialized in start
    private Rigidbody2D rb;
    private int jumpCounter;
    private float dashTime;
    private float dashTimeDelta;
    private bool isOnPlatform;

    // vars that are initialized elsewhere
    private Vector2 speed;
    private float horizontal;
    private bool canJump;
    private bool canDash;
    private int toggleNum;

    // start method
    private void Start()
    {
        respawner = GameObject.Find("GameController").GetComponent<EnemyRespawner>();
        rb = this.GetComponent<Rigidbody2D>();
        dashTime = 0.6f; // should last for approximately 60 frames
        dashTimeDelta = dashTime;
        jumpCounter = 0;
        jumpForce = 275f;
        playerSpeed = 3f;
        isGrounded = true;
        isFlipped = false;
        isOnPlatform = false;
        spawnLocation = this.transform.position;
        toggleNum = 3;
    }

    // update function
    private void Update()
    {
        // only here for testing purposes
        if (Input.GetKeyDown(KeyCode.R))
            RestartGameForTesting();
        // check if the player has fallen up the map
        if (this.transform.position.y < -1)
        {
            respawner.ResetEnemies();
            this.transform.position = spawnLocation;
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
            this.transform.Rotate(new Vector3(0, 180, 0));
            isFlipped = true;
        }
        else if (isFlipped && horizontal == 1)
        {
            this.transform.Rotate(new Vector3(0, -180, 0));
            isFlipped = false;
        }

        if (toggleNum == 1)
        {
            if (horizontal != -1)
            {
                horizontal = 0;
            }
        }
        else if (toggleNum == 2)
        {
            if (horizontal != 1)
            {
                horizontal = 0;
            }
        }

        // get the player speed
        if (canDash)
        {
            speed = new Vector2(horizontal * playerSpeed * 2, rb.velocity.y);
        }
        else
        {
            speed = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        }

        rb.velocity = speed;

        // Do jump if the player can jump
        if (canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jumpCounter++;
            canJump = false;
        }
    }

    // check for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            Physics2D.IgnoreCollision(
                this.gameObject.GetComponent<Collider2D>(),
                collision.collider
            );
        }
        if (collision.gameObject.CompareTag("PlayerSword"))
        {
            Physics2D.IgnoreCollision(
                this.gameObject.GetComponent<Collider2D>(),
                collision.collider
            );
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            canDash = false;
            Vector3 colliderPos = collision.collider.bounds.center;
            if (collision.gameObject.transform.position.x > this.transform.position.x)
            {
                toggleNum = 1;
            }
            else
            {
                toggleNum = 2;
            }
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCounter = 0;
            isGrounded = true;
            isOnPlatform = true;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            jumpCounter = 0;
            toggleNum = 3;
            isGrounded = true;
        }
    }

    // check for continous collisions
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.gameObject.transform.position.x > this.transform.position.x)
            {
                toggleNum = 1;
            }
            else
            {
                toggleNum = 2;
            }
        }
    }

    // check for the end of collisions
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
            isOnPlatform = false;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            toggleNum = 3;
        }
    }

    // check if entering certain triggers
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap"))
        {
            rb.velocity = new Vector2(0, 0);
            if (isFlipped)
            {
                this.transform.Rotate(new Vector3(0, -180, 0));
                isFlipped = false;
            }
            respawner.ResetEnemies();
            this.transform.position = spawnLocation;
        }
    }

    // return true if is grounded, else return false
    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    // return true if can dash, else return false
    public bool GetIfCanDash()
    {
        return canDash;
    }

    public bool GetIsOnPlatform()
    {
        return isOnPlatform;
    }

    public void RestartGameForTesting()
    {
        transform.position = spawnLocation;
        respawner.ResetEnemies();
    }

    public bool GetIsMoving()
    {
        return horizontal != 0;
    }
}
