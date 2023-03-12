using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{

    [SerializeField] GameObject player; 
    [SerializeField] GameObject enemy; 
    [SerializeField] GameObject projectile; 
 
    public Rigidbody2D rb; 
    private Vector3 direction; 
    private float timer = 5.0f; 
   
    void Update()
    {
        timer -= Time.deltaTime; 
        if (timer < 0)
        {
            timer = 5f;
            if (Mathf.Abs(player.transform.position.x - this.transform.position.x)< 5.0f)
            {
                Instantiate(projectile, enemy.transform.position, Quaternion.identity);
                rb = projectile.GetComponent<Rigidbody2D>(); 
            }
        }
    }    
}