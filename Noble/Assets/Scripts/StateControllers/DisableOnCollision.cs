using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class disables and enables a game object's sprite renderer
public class DisableOnCollision : MonoBehaviour
{
    // reference to self
    private GameObject self;

    // Start is called before the first frame update
    void Start()
    {
        self = this.gameObject;
    }

    // check for collisions with platforms and walls and disable the mesh renderer if so
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Platform") || collider.gameObject.CompareTag("Wall"))
        {
            // special case, keep the sprite renderer enabled, boundaries are invisible
            if (!collider.gameObject.name.Contains("Boundary"))
                this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // check for collision exits with platforms and walls and enable the mesh renderer if so
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Platform") || collider.gameObject.CompareTag("Wall"))
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
