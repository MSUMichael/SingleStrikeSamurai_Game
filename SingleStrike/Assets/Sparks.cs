using UnityEngine;

public class Sparks : MonoBehaviour
{
    void Start()
    {
        // Destroy the sparks GameObject after 0.1 seconds
        Destroy(gameObject, 0.5f);
    }
}

