using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectManager : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip die;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip swordScrape;


    public List<AudioClip> footstepSounds = new List<AudioClip>();
    public List<AudioClip> runningStepSounds = new List<AudioClip>();

    public Animator animator;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerJumpSound()
    {
        if (jump != null)
        {
            audioSource.PlayOneShot(jump);
        }
    }

    public void PlayerDieSound()
    {
        if (die != null)
        {
            audioSource.PlayOneShot(die);
        }
    }

    public void PlayeerAttack1Sound()
    {
        if (attack1 != null)
        {
            audioSource.PlayOneShot(attack1);
        }
    }

    public void PlayeerAttack2Sound()
    {
        if (attack2 != null)
        {
            audioSource.PlayOneShot(attack2);
        }
    }

    public void PlayRandomFootstepSound()
    {
        if (footstepSounds.Count > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Count);
            audioSource.PlayOneShot(footstepSounds[randomIndex]);
        }
    }

    public void PlayRandomRunningStepSound()
    {
        if (runningStepSounds.Count > 0)
        {
            int randomIndex = Random.Range(0, runningStepSounds.Count);
            audioSource.PlayOneShot(runningStepSounds[randomIndex]);
        }
    }

    public void PlaySwordScrape()
    {
       
            audioSource.PlayOneShot(swordScrape);
        
    }
}
