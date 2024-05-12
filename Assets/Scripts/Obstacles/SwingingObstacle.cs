using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit = 75f;
    public bool randomStart = false;
    private float random = 0;

    void Awake() 
    {
        if (randomStart)
        {
            random = Random.Range(0f , 5f);
        }

    }

    void Update() 
    {
        float angle = limit * Mathf.Sin((Time.time*speed) +  random);
        transform.localRotation = Quaternion.Euler(0,0,angle);
    }
}
