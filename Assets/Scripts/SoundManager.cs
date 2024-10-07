using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    // Start is called before the first frame update

    [SerializeField] private AudioClip _coinAudio;

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
    }

    public void CoinSFX()
    {
        _audioSource.PlayOneShot(_coinAudio);
    }
}
