using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by ryan reisdorf

public class EnemyWeaponController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject hitbox; // Reference to the hitbox GameObject

    public void EnableEnemyHitbox()
    {
        hitbox.SetActive(true); // Enable the hitbox
    }

    public void DisableEnemyHitbox()
    {
        hitbox.SetActive(false); // Disable the hitbox
    }

}
