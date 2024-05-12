using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public PickupTicket _pTk;
    public LockGates _lG;
    public GameObject DoorlockTooltip;
    public GameObject OpenedGates;
    bool gateopenable;
    public bool playerInside;
    public GameObject PrevFloorstars;

    // Start is called before the first frame update
    void Start()
    {  
        DoorlockTooltip.SetActive(false); 
        playerInside = true;
    }
    
        private void OnTriggerStay(Collider other)
    {
        gateopenable = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        DoorlockTooltip.SetActive(false);
        gateopenable = false;
    }

    void Update()
    {
        if ((_pTk.hasTicket)&&(gateopenable == true)&&(playerInside == true))
        {
                DoorlockTooltip.SetActive(true);

                if(Input.GetKey(KeyCode.E)) 
                {
                    Debug.Log("E pressed");

                    this.gameObject.SetActive(false);
                    OpenedGates.SetActive(true);

                    DoorlockTooltip.SetActive(false);
                    PrevFloorstars.SetActive(false);

                    playerInside = false;
                }
            
        }

    }
}
