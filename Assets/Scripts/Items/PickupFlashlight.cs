using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFlashlight : MonoBehaviour
{
    public GameObject PickupTooltip;
    public GameObject FlashlightOnPlayer;
    public GameObject Floorstars;
    public GameObject PrevFloorstars;

    public bool hasLight;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightOnPlayer.SetActive(false);    
        PickupTooltip.SetActive(false);    
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PickupTooltip.SetActive(true);

            if(Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);

                FlashlightOnPlayer.SetActive(true);

                PickupTooltip.SetActive(false);

                Floorstars.SetActive(true);
                PrevFloorstars.SetActive(false);

                hasLight = true;
            }
                if(hasLight == true)
            {
                Debug.Log("Flashlight Picked Up");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupTooltip.SetActive(false);
    }

}
