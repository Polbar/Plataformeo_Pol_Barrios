using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    // Start is called before the first frame update

    public AudioSource _audioSource;

    public AudioClip coinAudio;

    public AudioClip jumpAudio;

    public AudioClip attackAudio;

    public AudioClip attackspeedAudio;

    public AudioClip deathAudio;

    public AudioClip damageAudio;

    public AudioClip pauseAudio;

    public AudioClip mimikAudio;

    public AudioClip starAudio;

    public AudioClip healAudio;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }


    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
