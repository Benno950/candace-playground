using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockGates : MonoBehaviour
{

    public GameObject ClosedDoors;
    public GameObject DisableLamps;
    public GameObject Shutters;

    // Start is called before the first frame update
    void Start()
    {

    }

        private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        {
            this.gameObject.SetActive(false);
            ClosedDoors.SetActive(true);
            DisableLamps.SetActive(false);
            Shutters.tag = "ShutterDown";
        }
    }
}
