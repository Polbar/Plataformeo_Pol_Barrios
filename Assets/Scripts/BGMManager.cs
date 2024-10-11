using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BGMManager instance;

    private AudioSource _audioSource;

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
        _audioSource.loop = true;
        _audioSource.mute = false;
        _audioSource.volume = 1;
    }

    public void PlayBGM(AudioClip clip)
    {
        _audioSource.Play();
        _audioSource.clip = clip;
    }


    public void StopBGM()
    {
        _audioSource.Stop();
    }

    public void PauseBGM()
    {
        _audioSource.Pause();
    }
}
