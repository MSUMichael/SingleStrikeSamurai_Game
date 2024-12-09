using UnityEngine;
//Written by ryan reisdorf

public class Sparks : MonoBehaviour
{
    void Start()
    {
        // Destroy the sparks GameObject after 0.5 seconds
        Destroy(gameObject, 0.5f);
    }
}

