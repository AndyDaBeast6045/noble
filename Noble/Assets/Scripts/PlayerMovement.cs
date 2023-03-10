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
   
   
    private void Start()
    {
        jumpForce = 275f;
        dashForce = 1500f; 
        playerSpeed = 3f; 
        isGrounded = true;
        isFlipped = false; 
        spawnLocation = player.transform.position;

    }
    private void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.X) && isGrounded)
        {
            canJump = true;  
        } 

        if (Input.GetKeyUp(KeyCode.X))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2); 
        }


        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            canDash = true; 
        }

        if (Input.GetKeyUp(KeyCode.Z)) 
        {
            canDash = false; 
        }
    }    
    
        //rb.MovePosition(player.transform.position + horizontal * playerSpeed * Time.deltaTime);

    private void FixedUpdate()
    {

        Debug.Log("Is this executing");
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
            rb.AddForce(new Vector2(0, jumpForce)); 
            canJump = false; 
            isGrounded = false; 
        }

        if (canDash)
        {
            //Vector2 move = new Vector2(-1, 0) * speed / 2; 
            rb.velocity = new Vector2(horizontal * playerSpeed * 2, rb.velocity.y); 
            //canDash = false; 
        } 

    
        
    }
    private void LateUpdate() 
    {
        float offsetx = player.transform.position.x - Camera.transform.position.x;
        float offsety = (player.transform.position.y + 2f) - Camera.transform.position.y;
        
        if (Mathf.Abs(offsety) < 0.05f) offsety = 0;
        Camera.transform.Translate(new Vector2(offsetx,0));
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Platform")) isGrounded = true; 
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
