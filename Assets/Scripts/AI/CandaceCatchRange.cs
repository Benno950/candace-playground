using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandaceCatchRange : MonoBehaviour
{
    public DeathHandler deathHandlerScript;
    public CandaceAI candaceAI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        Debug.LogWarning("You are caught!");
        deathHandlerScript.CaughtByCandace();
        candaceAI.PlayerCaught();
    }

}
