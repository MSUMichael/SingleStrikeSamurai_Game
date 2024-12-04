using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by ryan reisdorf

public class BossWeaponController : MonoBehaviour
{
    
    public GameObject hitbox; // Reference to the hitbox GameObject

    public void EnableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true); // Enable the hitbox
        }
    }

    public void DisableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false); // Disable the hitbox
        }
    }
}



