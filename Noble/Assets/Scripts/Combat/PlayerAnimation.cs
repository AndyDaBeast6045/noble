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

    // player attack variables
    private int swordState;
    private string[] swingAnimations = { "SwordAttackA", "SwordAttackB", "SwordAttackC" };
    private Queue<string> attackInputBuffer = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject.GetComponent<PlayerMovement>();
        playerAnimator = this.gameObject.GetComponent<Animator>();
        swordState = 0;
        playerAnimator.SetFloat("SwordState", swordState);
    }

    // Update is called once per frame
    void Update()
    {
        // cancel attack animations, and reset them if the player leaves the ground or dashes
        if (!player.GetIsGrounded() || player.GetIfCanDash())
        {
            playerAnimator.Play("NobleIdle"); // Will be changed to jumping/dashing animation most likely
            while (attackInputBuffer.Count > 0)
                attackInputBuffer.Dequeue();
            swordState = 0;
            playerAnimator.SetFloat("SwordState", swordState);
            StopSwordResetTimer();
        }

        // begin an attack animation
        if (Input.GetKeyDown(KeyCode.C) && attackInputBuffer.Count == 0 && player.GetIsGrounded())
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

    // used by animation events to start a new sword timer
    private void StartSwordResetTimer()
    {
        StopCoroutine("WaitForSwordReset");
        StartCoroutine("WaitForSwordReset");
    }

    // used by animation events to stop the sword timer
    private void StopSwordResetTimer()
    {
        StopCoroutine("WaitForSwordReset");
    }

    // resets the sword state back to 0
    private void ResetSwordState()
    {
        swordState = 0;
        playerAnimator.SetFloat("SwordState", swordState);
    }

    public int GetCurrentSwordState()
    {
        return swordState; 
    }
}
