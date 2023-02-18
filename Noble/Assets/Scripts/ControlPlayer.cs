using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField] GameObject player; 
    [SerializeField] GameObject playerCamera;
    [SerializeField] Rigidbody2D rb; 
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float playerSpeed;
    [SerializeField] int playerDirection = 1; 
    [SerializeField] bool grounded = true; 


    private float horizontal; 
    // Update is called once per frame

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (horizontal == 1) playerDirection = 1; 
        if (horizontal == -1) playerDirection = -1; 

        player.transform.Translate(new Vector2(horizontal * playerSpeed * Time.deltaTime, 0));

       
        if (Input.GetKeyUp(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
            grounded = false; 
        } 
        if (Input.GetKeyUp(KeyCode.K)) rb.AddForce(transform.right * jumpForce * playerDirection);
    }
    private void LateUpdate() 
    {
        float offset = player.transform.position.x - playerCamera.transform.position.x;
        playerCamera.transform.Translate(new Vector2(offset,0));
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Platform")) grounded = true; 
    }
}