using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{

    public bool hideOnPlay = false;
    private MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("Appicable Mesh Renderer not found on object.");
        }

        if (hideOnPlay == true) meshRenderer.enabled = false;
    }

}
