using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the colliders of the player's weapon
public class PlayerAttack : MonoBehaviour
{
    // player weapon
    private GameObject sword;

    // instantiate player weapon variables
    void Start()
    {
        sword = this.gameObject.transform.GetChild(0).gameObject;
        DisableSwordCollider();
    }

    // enable the sword's collider
    public void EnableSwordCollider()
    {
        sword.GetComponent<Collider2D>().enabled = true;
    }

    // disable the sword's collider
    public void DisableSwordCollider()
    {
        sword.GetComponent<Collider2D>().enabled = false;
    }
}
