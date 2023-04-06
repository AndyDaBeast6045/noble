using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles all the animations related to the player
public class PlayerAnimation : MonoBehaviour
{
    // player movement script reference
    private PlayerMovement player;

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
    bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject.GetComponent<PlayerMovement>();
        playerAnimator = this.gameObject.GetComponent<Animator>();
        swordState = 0;
        playerAnimator.SetFloat("SwordState", swordState);
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        // cancel attack animations, and reset them if the player leaves the ground or dashes
        if (player.GetIsMoving())
        {
            playerAnimator.Play("NobleIdle"); // Will be changed to jumping/dashing animation most likely
            while (attackInputBuffer.Count > 0)
                attackInputBuffer.Dequeue();
            swordState = 0;
            playerAnimator.SetFloat("SwordState", swordState);
            StopSwordResetTimer();
        }

        // begin an attack animation
        if (
            Input.GetKeyDown(KeyCode.C)
            && attackInputBuffer.Count == 0
            && canAttack
            && !player.GetIsMoving()
        )
        {
            attackInputBuffer.Enqueue(swingAnimations[swordState]);
            playerAnimator.Play(swingAnimations[swordState]);
            swordState++;
            playerAnimator.SetFloat("SwordState", swordState);
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

    // get the current attack the player is executing if it is attacking
    public int GetCurrentAttack()
    {
        playerAnimatorInfo = this.playerAnimator.GetCurrentAnimatorClipInfo(0);
        currentAnimation = playerAnimatorInfo[0].clip.name;

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
