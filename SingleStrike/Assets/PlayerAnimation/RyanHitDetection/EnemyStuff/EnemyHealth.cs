using UnityEngine;
using System.Collections.Generic;
//Written by ryan reisdorf

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

    public AudioClip deathSound;
    private AudioSource audioSource2;
    private bool isDead = false;


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

        Debug.Log("Enemy is dying.");


        GetComponent<Collider>().enabled = false;


        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
            Debug.Log("Death sound should be playing now.");
        }


        Destroy(gameObject, deathSound.length);

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