using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // player weapons
    private GameObject sword;
    private GameObject melee; // will implemenmt at a later time

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
