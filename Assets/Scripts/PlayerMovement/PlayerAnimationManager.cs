using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    public Animator playerHandsAnimator;
    public bool allowInspect = true; 
    private float idleTimerRemaining = 5f;
    public float minIdleCooldown = 10f;
    public float maxIdleCooldown = 20f;

    void Update() 
    {
        InspectAnimationTimer();
    }

    // Update is called once per frame
    public void VaultingAnimation()
    {
        playerHandsAnimator.SetTrigger("TriggerVaulting");
    }

    public void JumpAnimation()
    {
        playerHandsAnimator.SetTrigger("Jump");
    }

    public void MovingAnimation()
    {
        playerHandsAnimator.SetBool("IsMoving", true);
    }

    public void SprintingAnimation()
    {
        playerHandsAnimator.SetBool("IsSprinting", true);
    }

    public void StopSprintingAnimation()
    {
        playerHandsAnimator.SetBool("IsSprinting", false);
    }

    public void BackToIdle()
    {
        playerHandsAnimator.SetBool("IsMoving", false);
    }

    public void DeathAnimation() 
    {
        playerHandsAnimator.SetTrigger("Death");
        //StartCoroutine(DisableAnimatorAfterDelay());
    }

    private void InspectAnimationTimer() 
    {
        if (allowInspect == false) 
        {
            return;
        }

        idleTimerRemaining -= Time.deltaTime;
        if (idleTimerRemaining <= 0f)
        {
            PlayIdleAnimation();
            idleTimerRemaining = Random.Range(minIdleCooldown, maxIdleCooldown); 
        }
    }

    public void PlayIdleAnimation() 
    {
        playerHandsAnimator.SetTrigger("InspectHands");
    }

    // private IEnumerator DisableAnimatorAfterDelay()
    // {
    //     // Wait for a specific amount of time (e.g., 1 second)
    //     yield return new WaitForSeconds(2f); // Adjust the delay as needed

    //     // Disable the animator after the delay
    //     playerHandsAnimator.enabled = false;
    // }

    
}
