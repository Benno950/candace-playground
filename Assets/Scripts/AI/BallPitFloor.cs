using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class BallPitFloor : MonoBehaviour
{

    private DeathHandler deathHandlerScript;
    private StarterAssets.FirstPersonController firstPersonController;

    public BallPitMonsterAI ballPitMonsterScript;

    [Header("Penalties")]
    public float slowdownSpeed = 2f;
    public float slowdownSprint = 6f;
    public float lowerJumpHeight = 0.2f;

    private float defaultSpeed;
    private float defaultSprint;
    private float defaultJumpHeight;

    void Start()
    {
        deathHandlerScript = GameObject.FindWithTag("Player").GetComponent<DeathHandler>();
        if (deathHandlerScript == null)
        {
            Debug.LogError("Error: DeathHandler Script not on player, dying is impossible!");
        }

        firstPersonController = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        if (firstPersonController == null)
        {
            Debug.LogError("FirstPersonController.cs component not found for ballpit.");
        }

        // Store the default values to recall them later
        defaultSpeed = firstPersonController.MoveSpeed;
        defaultSprint = firstPersonController.SprintSpeed;
        defaultJumpHeight = firstPersonController.JumpHeight;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        {
            deathHandlerScript.fallDamageEnable = false; // Stops fall damage as the ball pit absorbs falling damage
            ApplyMovementPenalty();
            AlertMonster();
            Debug.Log("Fell in ball pit");
        }
    }

     private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        {
            ballPitMonsterScript.AggressionTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving Ball Pit");
        if (other.CompareTag("Player")) // Only player can trigger this
        {
            CalmDownMonster();
            ClearMovementPenalty();
            ballPitMonsterScript.ResetAggression();
            deathHandlerScript.fallDamageEnable = true;
        }
    }

    private void ApplyMovementPenalty() 
    {
        firstPersonController.MoveSpeed = slowdownSpeed;
        firstPersonController.SprintSpeed = slowdownSprint;
        firstPersonController.JumpHeight = lowerJumpHeight;
    }

    private void ClearMovementPenalty() 
    {
        firstPersonController.MoveSpeed = defaultSpeed;
        firstPersonController.SprintSpeed = defaultSprint;
        firstPersonController.JumpHeight = defaultJumpHeight;
    }

    public void AlertMonster() 
    {
        ballPitMonsterScript.playerDetected = true;
        ballPitMonsterScript.ChasePlayer();
    }

    public void CalmDownMonster() 
    {
        ballPitMonsterScript.playerDetected = false;
    }
}
