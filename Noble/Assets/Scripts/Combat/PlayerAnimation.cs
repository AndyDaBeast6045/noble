using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles all the animations related to the player
public class PlayerAnimation : MonoBehaviour
{
    // player movement script reference
    private PlayerMovement player;

    // player rigidbody reference
    private Rigidbody2D rb;

    // player animation reference
    private Animator playerAnimator;
    private AnimatorClipInfo[] playerAnimatorInfo;
    private string currentAnimation;

    // refererences to coroutines that deal with timers
    private Coroutine swordExecuteTimer;
    private Coroutine swordAttackCooldown;

    // player attack variables
    private int swordState;
    private string[] swingAnimations = { "SwordAttackA", "SwordAttackB", "SwordAttackC" };
    private Queue<string> attackInputBuffer = new Queue<string>();
    private bool canAttack;
    private bool attackedInAir;
    private bool jumped;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAnimator.Play("NobleIdle");
        swordState = 0;
        playerAnimator.SetFloat("SwordState", swordState);
        canAttack = true;
        attackedInAir = false;
        jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // reset the attack queue if the player jumped and attacked
        if (player.GetIsGrounded() && attackedInAir)
        {
            resetAttackQueue();
        }
        else if (!player.GetIsGrounded() && !attackedInAir)
        {
            // execute jumping animations depending on rb velocity
            jumped = true; // will be used for the jumping animations

            if (rb.velocity.y > 0)
            {
                playerAnimator.Play("Jump");
            }
            else
            {
                //playerAnimator.Play("JumpDownward");
                // sprite needs to be fixed before it is used
            }
        }
        else
        {
            if (player.GetIsMoving() && player.GetIsGrounded())
            {
                // play dash or run animation
                if (player.GetIfCanDash())
                {
                    playerAnimator.Play("NobleDash");
                }
                else
                {
                    playerAnimator.Play("NobleRunning");
                }

                // reset attack queue and reset the sword state
                resetAttackQueue();
                ResetSwordState();
            }
            else if (swordState == 0 && !attackedInAir)
            {
                playerAnimator.Play("NobleIdle");
            }
        }

        // begin an attack animation
        if (
            Input.GetKeyDown(KeyCode.C)
            && attackInputBuffer.Count == 0
            && canAttack
            && !player.GetIsMoving()
            && player.GetIsGrounded()
        )
        {
            attackInputBuffer.Enqueue(swingAnimations[swordState]);
            playerAnimator.Play(swingAnimations[swordState]);
            swordState++;
            playerAnimator.SetFloat("SwordState", swordState);
        }
        else if (
            Input.GetKeyDown(KeyCode.C) && canAttack && !player.GetIsGrounded() && !attackedInAir
        )
        {
            playerAnimator.Play("JumpingSwordAttack");
            attackedInAir = true;
        }
    }

    // used by attack animation events to remove themselves from the input buffer
    private void RemoveAttackFromInputBuffer()
    {
        if (attackInputBuffer.Count > 0)
            attackInputBuffer.Dequeue();
    }

    // time window started to execute another sword attack or the sequence restarts
    private IEnumerator WaitForSwordReset()
    {
        yield return new WaitForSeconds(0.75f);
        swordState = 0;
        playerAnimator.SetFloat("SwordState", swordState);
    }

    // cool down period after the full attack animation
    private IEnumerator WaitForAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    // used by an animation event to start the sword attack cooldown
    public void StartAttackCooldownTimer()
    {
        if (swordAttackCooldown != null)
            StopCoroutine(swordAttackCooldown);
        StartCoroutine(WaitForAttackCooldown());
    }

    // used by an animation event to start a new sword timer
    public void StartSwordResetTimer()
    {
        if (swordExecuteTimer != null)
            StopCoroutine(swordExecuteTimer);
        swordExecuteTimer = StartCoroutine(WaitForSwordReset());
    }

    // used by an animation event to stop the sword timer
    public void StopSwordResetTimer()
    {
        if (swordExecuteTimer != null)
            StopCoroutine(swordExecuteTimer);
    }

    // resets the sword state back to 0
    public void ResetSwordState()
    {
        swordState = 0;
        playerAnimator.SetFloat("SwordState", swordState);
    }

    // empty the attack queue and switch jumping to false
    public void resetAttackQueue()
    {
        while (attackInputBuffer.Count > 0)
            attackInputBuffer.Dequeue();
        if (attackedInAir)
            attackedInAir = false;
        if (jumped)
            jumped = false;
    }

    // get the current attack the player is executing if it is attacking
    public int GetCurrentAttack()
    {
        playerAnimatorInfo = this.playerAnimator.GetCurrentAnimatorClipInfo(0);
        if (playerAnimatorInfo.Length > 0)
        {
            currentAnimation = playerAnimatorInfo[0].clip.name;
        }
        else
        {
            currentAnimation = "";
        }

        switch (currentAnimation)
        {
            case "SwordAttackA":
                return 1;
            case "SwordAttackB":
                return 2;
            case "SwordAttackC":
                return 3;
            default:
                return 0;
        }
    }
}
