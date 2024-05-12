using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupKey : MonoBehaviour
{
    public GameObject PickupTooltip;
    public bool hasKey;
    public GameObject Floorstars;

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
                hasKey = true;
                PickupTooltip.SetActive(false);
                Floorstars.SetActive(true);
            }
            if(hasKey == true)
            {
                Debug.Log("Haskey");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupTooltip.SetActive(false);
    }
}
