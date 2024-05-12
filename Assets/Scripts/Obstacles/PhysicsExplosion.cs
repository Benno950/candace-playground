using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsExplosion : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 20.0F;
    private bool explosionOccurred = false; // Flag to track if explosion has occurred

    private Color gizmoColor = Color.red;

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!explosionOccurred && other.CompareTag("Player")) // Only player can trigger this and explosion hasn't occurred yet
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
            
            // Set the flag to true to indicate that the explosion has occurred
            explosionOccurred = true;
            
            Debug.Log("Explosion");
        }
    }
}
