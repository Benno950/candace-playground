using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutNet : MonoBehaviour
{
    public PickupScissor _pS;
    public GameObject NetCutTooltip;
    bool cuttable;

    // Start is called before the first frame update
    void Start()
    {  
        NetCutTooltip.SetActive(false); 
    }
    
        private void OnTriggerStay(Collider other)
    {
        cuttable = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        NetCutTooltip.SetActive(false);
        cuttable = false;
    }

    void Update()
    {
        if ((_pS.hasKnife)&&(cuttable == true))
        {
                Debug.Log("Scissors on trigger");
                NetCutTooltip.SetActive(true);

                if(Input.GetKey(KeyCode.E)) 
                {
                    Debug.Log("E pressed");

                    this.gameObject.SetActive(false);

                    NetCutTooltip.SetActive(false);
                }
            
        }

    }





}
