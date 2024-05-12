using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandaceAnimations : MonoBehaviour
{
    public Animator candaceAnimator;   
     void Update()
    {
        
    }

    public void Idle() 
    {
        candaceAnimator.SetBool("Idle", true);
    }

    public void StopIdle() 
    {
        candaceAnimator.SetBool("Idle", false);
    }


    public void Wander() 
    {
        candaceAnimator.SetBool("Wandering", true);
    }

    public void StopWander() 
    {
        candaceAnimator.SetBool("Wandering", false);
    }

    public void Chase() 
    {
        candaceAnimator.SetBool("Chasing", true);
    }
    public void StopChase() 
    {
        candaceAnimator.SetBool("Chasing", false);   
    }

    public void Caught() 
    {
        candaceAnimator.SetTrigger("Caught");
    }

    // Update is called once per frame

}
