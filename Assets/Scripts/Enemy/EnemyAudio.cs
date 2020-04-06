using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip screamClip, dieClip;
    [SerializeField] AudioClip[] attackClips;
    
    private AudioSource audioSource;



    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }   

    public void PlayAttackSound()
    {
        audioSource.clip = attackClips[Random.Range(0, attackClips.Length)];
        audioSource.Play();
    }

    public void PlayDeadSound()
    {
        audioSource.clip = dieClip;
        audioSource.Play();

    }

    public void PlayScreamSound()
    {
        audioSource.clip = screamClip;
        audioSource.Play();

    }
}
