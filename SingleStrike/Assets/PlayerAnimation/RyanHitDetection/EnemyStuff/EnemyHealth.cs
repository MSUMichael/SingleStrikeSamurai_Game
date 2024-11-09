/*
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public static int currentHealth;
    public Animator animator;
    public AudioClip EnemyDeathSound; // Sound effect for boss dying
    private AudioSource audioSource; // Reference to the AudioSource

    private GameObject bloodSprayPrefab; // Reference to the Blood Spray prefab
    public GameObject bloodSprayPrefab0;
    public GameObject bloodSprayPrefab1;
    public GameObject bloodSprayPrefab2;

    private GameObject bloodPoolPrefab; // Reference to the Blood Pool prefab
    public GameObject bloodPoolPrefab0;
    public GameObject bloodPoolPrefab1;
    public GameObject bloodPoolPrefab2;
    public GameObject bloodPoolPrefab3;

    public Transform bloodSpawnPoint;   // Point where the blood spray will be spawned

    private bool hasDied;

    private int rndSpray;
    private int rndPool;
    private int rndSprayRot;
    private int rndPoolRot;
    public Vector3 bloodScale;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Destroy the enemy GameObject or trigger a death animation
        Debug.Log("Enemy died.");
        LockPosition();
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false; // Disable the enemy's collider to prevent further interactions
        this.enabled = false;
        
        EnemyBloodSpray();
        EnemyBloodPool();
        
        Destroy(gameObject, 15f);
    }
    private void LockPosition()
    {
        // Disable any Rigidbody to prevent further movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Make Rigidbody kinematic to stop physics from affecting it
        }

        // Optionally, disable other scripts that could affect the transform
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Do not disable the current EnemyHealth script
            {
                script.enabled = false;
            }
        }

        // Optionally, freeze the transform by disabling all constraints
        transform.hasChanged = false; // Reset any pending changes to the transform
    }

    public void EnemyBloodSpray()
    {

        rndSprayRot = 0;//Random.Range(0, 359);

        rndSpray = Random.Range(0, 2);

        switch (rndSpray)
        {
            case 0:
                bloodSprayPrefab = bloodSprayPrefab0;
                break;
            case 1:
                bloodSprayPrefab = bloodSprayPrefab1;
                break;
            case 2:
                bloodSprayPrefab = bloodSprayPrefab2;
                break;
            default:
                bloodSprayPrefab = bloodSprayPrefab0;
                break;
        }

        if (bloodSprayPrefab != null && bloodSpawnPoint != null)
        {
            GameObject sprayInstance = Instantiate(bloodSprayPrefab, bloodSpawnPoint.position, Quaternion.Euler(new Vector3(rndSprayRot, 90, -90)));
            sprayInstance.transform.localScale = bloodScale;
            Debug.Log("Blood spray created.");

        }
    }

    public void EnemyBloodPool()
    {

        rndPoolRot = Random.Range(0, 359);

        rndPool = Random.Range(0, 3);

        switch (rndPool)
        {
            case 0:
                bloodPoolPrefab = bloodPoolPrefab0;
                break;
            case 1:
                bloodPoolPrefab = bloodPoolPrefab1;
                break;
            case 2:
                bloodPoolPrefab = bloodPoolPrefab2;
                break;
            case 3:
                bloodPoolPrefab = bloodPoolPrefab3;
                break;
            default:
                bloodPoolPrefab = bloodPoolPrefab0;
                break;
        }

        if (bloodPoolPrefab != null)
        {
            GameObject poolInstance = Instantiate(bloodPoolPrefab, transform.position, Quaternion.Euler(new Vector3(0, rndPoolRot, 0)));
            poolInstance.transform.localScale = bloodScale*2;
            Debug.Log("Blood pool created.");
        }
    }

}
*/

using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public static int currentHealth;
    public Animator animator;
    public AudioClip EnemyDeathSound; // Sound effect for boss dying
    private AudioSource audioSource; // Reference to the AudioSource

    public List<GameObject> bloodSprayPrefabs; // List for Blood Spray prefabs
    public List<GameObject> bloodPoolPrefabs;  // List for Blood Pool prefabs

    public Transform bloodSpawnPoint; // Point where the blood spray will be spawned
    public Vector3 bloodScale;

    private bool hasDied;

    private int rndSpray;
    private int rndPool;
    private int rndSprayRot;
    private int rndPoolRot;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");
        LockPosition();
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false; // Disable the enemy's collider to prevent further interactions
        this.enabled = false;

        EnemyBloodSpray();
        EnemyBloodPool();

        Destroy(gameObject, 15f);
    }

    private void LockPosition()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Make Rigidbody kinematic to stop physics from affecting it
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Do not disable the current EnemyHealth script
            {
                script.enabled = false;
            }
        }

        transform.hasChanged = false; // Reset any pending changes to the transform
    }

    public void EnemyBloodSpray()
    {
        rndSprayRot = 0; // Rotation for the blood spray
        rndSpray = Random.Range(0, bloodSprayPrefabs.Count); // Randomly select a blood spray prefab

        if (bloodSprayPrefabs.Count > 0 && bloodSpawnPoint != null)
        {
            GameObject sprayInstance = Instantiate(bloodSprayPrefabs[rndSpray], bloodSpawnPoint.position, Quaternion.Euler(new Vector3(rndSprayRot, 90, -90)));
            sprayInstance.transform.localScale = bloodScale;
            Debug.Log("Blood spray created.");
        }
    }

    public void EnemyBloodPool()
    {
        rndPoolRot = Random.Range(0, 359); // Rotation for the blood pool
        rndPool = Random.Range(0, bloodPoolPrefabs.Count); // Randomly select a blood pool prefab

        if (bloodPoolPrefabs.Count > 0)
        {
            GameObject poolInstance = Instantiate(bloodPoolPrefabs[rndPool], transform.position, Quaternion.Euler(new Vector3(0, rndPoolRot, 0)));
            poolInstance.transform.localScale = bloodScale * 2;
            Debug.Log("Blood pool created.");
        }
    }
}
