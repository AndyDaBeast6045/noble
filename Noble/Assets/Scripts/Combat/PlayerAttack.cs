using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnimator;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.gameObject.GetComponent<Animator>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isAttacking)
        {
            playerAnimator.Play("SwingAnimation");
        }
    }
}
