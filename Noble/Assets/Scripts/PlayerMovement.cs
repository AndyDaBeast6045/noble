using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] GameObject playerCamera;
    [SerializeField] Rigidbody2D rb; 
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float playerSpeed;
    [SerializeField] int playerDirection; 
    [SerializeField] bool grounded = true; 


    private float horizontal; 
    private float cameraYPos;
    bool canJump;
    bool canDash; 
   
    private void Start()
    {
        jumpForce = 350f;
        dashForce = 30f; 
        playerSpeed = 3f; 
        playerDirection = 1; 
        cameraYPos = 3f;

    }
    private void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyUp(KeyCode.Space) && grounded)
        {
            canJump = true;  
        } 
        if (Input.GetKeyUp(KeyCode.K)) 
        {
            canDash = true; 
        }
    }    
    
        //rb.MovePosition(player.transform.position + horizontal * playerSpeed * Time.deltaTime);

    private void FixedUpdate()
    {

        Debug.Log("Is this executing");
        
        if (horizontal == 1) playerDirection = 1; 
        if (horizontal == -1) playerDirection = -1; 

        Vector2 speed = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        rb.velocity = Vector2.MoveTowards(rb.velocity, speed, 10f);

        if (canJump) 
        {
            rb.AddForce(new Vector2(0, jumpForce)); 
            canJump = false; 
            grounded = false; 
        }

        if (canDash)
        {
            rb.AddForce(new Vector2(playerDirection * dashForce, 0));
            canDash = false; 
        }
    }
    private void LateUpdate() 
    {
        float offsetx = player.transform.position.x - playerCamera.transform.position.x;
        float offsety = (player.transform.position.y + 2f) - playerCamera.transform.position.y;
        
        if (Mathf.Abs(offsety) < 0.05f) offsety = 0;
        playerCamera.transform.Translate(new Vector2(offsetx,0));
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Platform")) grounded = true; 
    }

}
