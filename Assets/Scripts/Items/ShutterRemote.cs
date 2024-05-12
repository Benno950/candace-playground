using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterRemote : MonoBehaviour
{
        public GameObject PickupTooltip;
        public GameObject Shutters;

        public GameObject shutterInvisibleWall;
    // Start is called before the first frame update
    void Start()
    {
        PickupTooltip.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PickupTooltip.SetActive(true);

            if(Input.GetKey(KeyCode.E))
            {
                shutterInvisibleWall.SetActive(false);
                this.gameObject.SetActive(false);   
                Shutters.tag = "ShutterUp";
                PickupTooltip.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupTooltip.SetActive(false);
    }
}
