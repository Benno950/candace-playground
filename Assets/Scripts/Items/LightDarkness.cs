using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDarkness : MonoBehaviour
{
public PickupFlashlight _pF;

    // Start is called before the first frame update
    void Start()
    {  

    }

    void Update()
    {
        if (_pF.hasLight)
        {
        this.gameObject.SetActive(false);
        }
    }
}
