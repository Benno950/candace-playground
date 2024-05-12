using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{

    private DeathHandler deathHandlerScript; 

    void Start() 
    {
        deathHandlerScript = GameObject.FindWithTag("Player").GetComponent<DeathHandler>();
        if (deathHandlerScript == null)
        {
            Debug.LogError("Error: DeathHandler Script not on player, dying is impossible!");
        }
    }


    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        {
         Debug.LogWarning ("You entered the kill area.");
         deathHandlerScript.BlockAllMovement();
         deathHandlerScript.RagdollPlayer();
         deathHandlerScript.FadeToBlack();
        }
    }
}