using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandaceArea : MonoBehaviour
{
    public CandaceAI candaceAIScript;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering area");
        if (other.CompareTag("Player")) // Only player can trigger this
        {
            Debug.Log("Enter chase");
            // Set playerDetected to true in the CandaceAI script
            candaceAIScript.playerDetected = true;
            candaceAIScript.ChasePlayer();
            Debug.Log(candaceAIScript.playerDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("LeavingArea");
        if (other.CompareTag("Player")) // Only player can trigger this
        {
            candaceAIScript.playerDetected = false;
        }
    }
}
