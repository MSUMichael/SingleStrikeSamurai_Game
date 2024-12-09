using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
//Written entirely by Michael Anglemier

public class PassiveMusicPlayer : MonoBehaviour
{
    public AudioClip passiveSound1;
    public AudioClip passiveSound2;
    public AudioClip passiveSound3;
    public AudioClip passiveSound4;
    public AudioClip dramaticDefault;
    private AudioClip passiveSound;
    public AudioSource audioSource;

    private bool enemyNear = false;

    private GameObject player;
    private Vector3 PlayerCoords;

    void Awake()
    {
        player = GameObject.Find("Main Camera");
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = passiveSound1;
        audioSource.volume = 1.0f;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        PlayerCoords = player.transform.position;

        if (PlayerCoords.x <= 50)
        {
            passiveSound = passiveSound1;
        }
        else if (PlayerCoords.x > 50 && PlayerCoords.x < 260)
        {
            passiveSound = passiveSound2;
        }
        else if (PlayerCoords.x >=260 && PlayerCoords.x < 690)
        {
            passiveSound = passiveSound3;
        }
        else if (PlayerCoords.x >= 690)
        {
            passiveSound = passiveSound4;
        }
        else
        {
            passiveSound = dramaticDefault;
        }
    }


    //Playing

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("EnemyHurtbox"))
        {
            StartCoroutine(FadeAudioSource.StartFade(audioSource, 2.0f, 0));

            enemyNear = true;
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
        if (other.CompareTag("EnemyHurtbox") || enemyNear==false)
        {
            audioSource.clip = passiveSound;
            audioSource.volume = 1.0f;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

}


