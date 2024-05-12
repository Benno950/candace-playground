using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTicket : MonoBehaviour
{
    public GameObject PickupTooltip;
    public bool hasTicket;
    public GameObject Floorstars;
    public GameObject PrevFloorstars;
    public GameObject Exitlamp;

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
                this.gameObject.SetActive(false);
                hasTicket = true;
                PickupTooltip.SetActive(false);
                Floorstars.SetActive(true);
                PrevFloorstars.SetActive(false);
                Exitlamp.SetActive(true);
            }
            if(hasTicket == true)
            {
                Debug.Log("Ticket Picked Up");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupTooltip.SetActive(false);
    }
}
