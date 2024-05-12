using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;



public class DebugText1 : MonoBehaviour
{
    public FirstPersonController scriptHere;
    public DeathHandler deathHandlerHere;
    TMP_Text actualText;


    void Start() 
    {
        actualText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        actualText.text = scriptHere._verticalVelocity.ToString();

        if (scriptHere._verticalVelocity < deathHandlerHere.FatalFallSpeed) {
            actualText.color = Color.red;  
        }
        else {
            actualText.color = Color.white;  
        }
    }

}
