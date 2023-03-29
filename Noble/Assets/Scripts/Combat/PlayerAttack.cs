using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject sword;
    private GameObject melee; // will implemenmt at a later time

    void Start()
    {
        sword = this.gameObject.transform.GetChild(0).gameObject;
        sword.GetComponent<Collider2D>().enabled = false;
    }

    public void EnableSwordCollider()
    {
        sword.GetComponent<Collider2D>().enabled = true;
    }

    public void DisableSwordCollider()
    {
        sword.GetComponent<Collider2D>().enabled = false;
    }
}
