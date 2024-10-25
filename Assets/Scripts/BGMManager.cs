// BGMManager.cs
using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource _audioSource;

    void Awake()
    {
        // Instancia única del BGMManager para todas las escenas excepto Main Menu
        if (instance != null && instance != this)
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

    // Inicializa la música de fondo
    public void PlayBGM(AudioClip clip)
    {
        _audioSource.Stop();  // Asegurarse de que el audio se detenga antes de cambiar
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void StopBGM()
    {
        _audioSource.Stop();
    }

    public void ResetInstance()
    {
        instance = null;
    }
}
