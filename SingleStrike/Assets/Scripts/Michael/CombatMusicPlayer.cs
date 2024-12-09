using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
//Written entirely by Michael Anglemier
//For some reason this code works great with the archer enemies, not so well with the others, unsure why.

public class CombatMusicPlayer : MonoBehaviour
{
    private bool enemyNear = false;
    private bool isPlaying = false;

    public AudioClip combatSound;
    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = combatSound;
        audioSource.volume = 1.0f;
        audioSource.loop = true;
    }

    

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("EnemyHurtbox") && isPlaying==false)
        {
            audioSource.volume = 1.0f;
            audioSource.Play();
            isPlaying = true;
        }
    }

    void OnTriggerStay(Collider other)

    {

        if (other.CompareTag("EnemyHurtbox"))
        {
            enemyNear = true;
        }
        else
        {
            enemyNear = false;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyHurtbox") && enemyNear==false)
        {
            StartCoroutine(FadeAudioSource.StartFade(audioSource, 2.0f, 0));
            isPlaying = false;
        }

    }

}


