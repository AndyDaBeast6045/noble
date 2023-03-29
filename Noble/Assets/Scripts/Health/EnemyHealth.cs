using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;

    private float currentHealth;
    private GameObject enemy;
    private GameObject player; 
    private PlayerAnimation playerAnimation; 

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.GetComponent<GameObject>();
        player = GameObject.Find("Player"); 
        playerAnimation = player.GetComponent<PlayerAnimation>(); 
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemy Current Health is..." + currentHealth);
        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerSword"))
        {
            if (playerAnimation.GetCurrentSwordState() < 2)
            {
                currentHealth -= 50; 
            }
            else 
            {
                currentHealth -= 100; 
            }
            collision.collider.enabled = false;
        }
    }
}
