using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public PickupKey _pK;
    public GameObject DoorlockTooltip;
    bool openable;

    // Start is called before the first frame update
    void Start()
    {  
        DoorlockTooltip.SetActive(false); 
    }
    
        private void OnTriggerStay(Collider other)
    {
        openable = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        DoorlockTooltip.SetActive(false);
        openable = false;
    }

    void Update()
    {
        if ((_pK.hasKey)&&(openable == true))
        {
                Debug.Log("Scissors on trigger");
                DoorlockTooltip.SetActive(true);

                if(Input.GetKey(KeyCode.E)) 
                {
                    Debug.Log("E pressed");

                    this.gameObject.SetActive(false);

                    DoorlockTooltip.SetActive(false);
                }
            
        }

    }
}
