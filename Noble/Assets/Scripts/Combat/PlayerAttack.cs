using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnimator;
    private bool isAttacking;
    private int swordState;
    private string[] swingAnimations = { "SwordAttackA", "SwordAttackB", "SwordAttackC" };
    private Queue<string> attackInputBuffer = new Queue<string>();

    AnimatorClipInfo[] animatorInfo;
    string currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        swordState = 0;
        playerAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            attackInputBuffer.Enqueue(swingAnimations[swordState]);
            playerAnimator.Play(swingAnimations[swordState]);
            swordState++;
            animatorInfo = this.playerAnimator.GetCurrentAnimatorClipInfo(0);
            currentAnimation = animatorInfo[0].clip.name;
            Debug.Log(currentAnimation);
        }
    }

    private void RemoveAttackFromQueue()
    {
        // attackInputBuffer.Dequeue();
    }

    private IEnumerator WaitForSwordReset()
    {
        yield return new WaitForSeconds(2);
        swordState = 0;
    }

    private void StartSwordResetTimer()
    {
        StopCoroutine("WaitForSwordReset");
        StartCoroutine("WaitForSwordReset");
    }

    private void StopSwordResetTimer()
    {
        StopCoroutine("WaitForSwordReset");
    }

    private void ResetSwordState()
    {
        swordState = 0;
    }
}
