using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int coins = 0;
    private int starsCollected = 0; // Contador de estrellas

    private bool isPaused;

    [SerializeField] Text _coinText;
    [SerializeField] GameObject _pauseCanvas;

    // Array de referencias a los objetos de estrellas
    [SerializeField] GameObject[] estrellasActivas; // Cambia a un array

    private Animator _pausePanelAnimator;

    private bool pauseAnimator;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        _pausePanelAnimator = _pauseCanvas.GetComponentInChildren<Animator>();
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            _pauseCanvas.SetActive(true);
        }
        else if (isPaused && !pauseAnimator)
        {
            pauseAnimator = true;
            StartCoroutine(ClosePauseAnimation());
        }
    }

    IEnumerator ClosePauseAnimation()
    {
        _pausePanelAnimator.SetBool("Close", true);
        yield return new WaitForSecondsRealtime(0.50f);
        Time.timeScale = 1;
        _pauseCanvas.SetActive(false);
        isPaused = false;
        pauseAnimator = false;
    }

    public void AddCoin()
    {
        coins++;
        _coinText.text = coins.ToString();
    }

    public void AddStar()
    {
        // Incrementar el contador de estrellas recogidas
        starsCollected++;

        // Activar la estrella correspondiente si existe
        if (starsCollected - 1 < estrellasActivas.Length)
        {
            estrellasActivas[starsCollected - 1].SetActive(true);
        }
    }
}
