using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    private Rigidbody2D rb; 
    private GameObject player; 
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        player = GameObject.FindGameObjectWithTag("Player"); 
        Vector3 direction = player.transform.position - this.transform.position; 
        rb.velocity = new Vector2(direction.x, direction.y).normalized * 5;
    }

    
}
