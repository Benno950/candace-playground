using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToEndscreen : MonoBehaviour
{

    private DeathHandler deathHandlerScript; 
    public GameObject Endscreen;


    void Start()
    {
        deathHandlerScript = GameObject.FindWithTag("Player").GetComponent<DeathHandler>();
        if (deathHandlerScript == null)
        {
            Debug.LogError("Error: DeathHandler Script not on player, dying is impossible!");
        }
                 Endscreen.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        {
         Debug.LogWarning ("You entered the endgame hitbox.");
         deathHandlerScript.BlockAllMovement();
         Endscreen.SetActive(true);
        }
    }
}
