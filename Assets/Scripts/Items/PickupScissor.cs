using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScissor : MonoBehaviour
{
    public GameObject PickupTooltip;
    public bool hasKnife;
    public GameObject Floorstars;
    public GameObject PrevFloorstars;

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
                hasKnife = true;
                PickupTooltip.SetActive(false);
                Floorstars.SetActive(true);
                PrevFloorstars.SetActive(false);
            }
            if(hasKnife == true)
            {
                Debug.Log("Scissors Picked Up");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupTooltip.SetActive(false);
    }
}
