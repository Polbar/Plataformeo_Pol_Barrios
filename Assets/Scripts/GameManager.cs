using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int coins = 0;
    private int starsCollected = 0; // Contador de estrellas
    private bool isPaused;

    [SerializeField] Text _coinText;
    [SerializeField] GameObject _pauseCanvas;
    
    // Nueva referencia al Canvas Victory
    [SerializeField] GameObject _victoryCanvas;

    // Array de referencias a los objetos de estrellas
    [SerializeField] GameObject[] estrellasActivas;

    private Animator _pausePanelAnimator;
    private bool pauseAnimator;

    [SerializeField] private Slider _healthBar;

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

        // Restablecer el timeScale para asegurarte de que el juego no esté pausado
        Time.timeScale = 1;

        _pausePanelAnimator = _pauseCanvas.GetComponentInChildren<Animator>();

        // Asegurarse de que el Canvas Victory esté desactivado al inicio
        _victoryCanvas.SetActive(false);
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

        // Comprobar si se han recogido 4 estrellas
        if (starsCollected >= 4)
        {
            // Activar el Canvas Victory
            _victoryCanvas.SetActive(true);

            // Opcional: Pausar el juego si deseas mostrar la victoria como una pantalla final
            Time.timeScale = 0;
        }
    }

    public void UpdateHealthBar(int health)
    {
        _healthBar.value = health;
    }

    public void SetHealthBar(int maxHealth)
    {
        _healthBar.maxValue = maxHealth;
        _healthBar.value = maxHealth;
    }

    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
