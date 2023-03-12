using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb; 
    [SerializeField] GameObject player; 
    [SerializeField] float speed; 
    [SerializeField] bool isRotated; 

    // Start is called before the first frame update
    void Start()
    {
       speed = 4f;  
       isRotated = false; 
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < 5f)
        {
            rb.AddForce(new Vector3(speed, 0));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > this.transform.position.x && isRotated)
        {
            this.transform.Rotate(new Vector3(0, 180, 0)); 
            speed = -speed;
            isRotated = false; 
        }
        if (player.transform.position.x < this.transform.position.x && !isRotated)
        {
            this.transform.Rotate(new Vector3(0, -180, 0)); 
            speed = -speed; 
            isRotated = true; 
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap")) Destroy(this.gameObject); 
    }
}
