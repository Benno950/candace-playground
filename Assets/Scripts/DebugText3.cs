using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;



public class DebugText3 : MonoBehaviour
{
    public CharacterController gameObjectHere;
    TMP_Text actualText;


    void Start() 
    {
        actualText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        actualText.text = gameObjectHere.velocity.normalized.ToString(); 
    }

}
