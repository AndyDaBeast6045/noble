using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] Camera Camera; 
    [SerializeField] Rigidbody2D rb; 
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float playerSpeed;
    [SerializeField] bool isGrounded;  
    [SerializeField] bool isFlipped; 
    [SerializeField] Vector2 spawnLocation; 

    private float horizontal; 
    bool canJump;
    bool canDash; 
    int jumpCounter; 
    float coyoteTime;
    float coyoteTimeDelta; 
   
    private void Start()
    {
        coyoteTime = 0.2f;
        coyoteTimeDelta = coyoteTime; 
        jumpCounter = 0; 
        jumpForce = 275f;
        playerSpeed = 3f; 
        isGrounded = true;
        isFlipped = false; 
        spawnLocation = player.transform.position;

    }
    private void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");

        if (isGrounded) 
        {
            coyoteTimeDelta = coyoteTime;
        } 
        else if (jumpCounter == 0)
        {
            coyoteTimeDelta -= Time.deltaTime; 
        }

        if (Input.GetKeyDown(KeyCode.X) && jumpCounter < 2 && (coyoteTimeDelta > 0 || jumpCounter > 0))
        {
            canJump = true;  
        } 
        
        if (Input.GetKeyUp(KeyCode.X))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2); 
            jumpCounter++; 
        }

        if (Input.GetKeyDown(KeyCode.Z) && jumpCounter == 0) 
        {
            canDash = true; 
        }

        if (Input.GetKeyUp(KeyCode.Z)) canDash  = false;
    }    
    
    private void FixedUpdate()
    {
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

        Vector2 speed = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        rb.velocity = Vector2.MoveTowards(rb.velocity, speed, 1f);
        if (canJump) 
        {
            rb.gravityScale = 1;
            rb.AddRelativeForce(new Vector2(0, jumpForce));
            canJump = false; 
            coyoteTimeDelta = 0;  
        }

        if (canDash)
        { 
            rb.AddForce(new Vector2(horizontal * 800, 0) ); 
            canDash = false; 
        } 
    }
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

        float offsetx = player.transform.position.x - Camera.transform.position.x;
        float offsety = (player.transform.position.y + 2f) - Camera.transform.position.y;
        
        if (Mathf.Abs(offsety) < 0.05f) offsety = 0;
        Camera.transform.Translate(new Vector2(offsetx,0));
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Platform")) 
        { 
            jumpCounter = 0; 
            isGrounded = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) isGrounded = false; 
    }

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
    }

}
