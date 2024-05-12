using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startscreen : MonoBehaviour
{

    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if(Input.anyKey)
            {
                this.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
    }

}
