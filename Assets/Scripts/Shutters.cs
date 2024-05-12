using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutters : MonoBehaviour
{
    void Update()
    {
        CheckTag();
    }

    void CheckTag()
    {
        if(this.gameObject.tag == "ShutterUp")
        {
            ShutterLift();
        }
        if(this.gameObject.tag == "ShutterDown")
        {
            ShutterFall();
        }
    }

    void ShutterLift()
    {
            if(transform.position.y <= 5f)
            {
                transform.Translate(0,(1 * Time.deltaTime),0);
            }
    }
        void ShutterFall()
    {
            if(transform.position.y > 0)
            {
                transform.Translate(0,(-10 * Time.deltaTime),0);
            }
    }
}
