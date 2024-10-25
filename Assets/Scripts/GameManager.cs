// GameManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int coins = 0;
    private int starsCollected = 0;
    private bool isPaused;

    [SerializeField] Text _coinText;
    [SerializeField] GameObject _pauseCanvas;
    [SerializeField] GameObject _victoryCanvas;
    [SerializeField] GameObject[] estrellasActivas;
    private Animator _pausePanelAnimator;
    private bool pauseAnimator;
    [SerializeField] private Slider _healthBar;

    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip gameOverBGM;
    [SerializeField] private AudioClip scene1BGM;
    [SerializeField] private AudioClip victoryClip;

    private BGMManager bgmManager;

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

        Time.timeScale = 1;
        _pausePanelAnimator = _pauseCanvas.GetComponentInChildren<Animator>();
        _victoryCanvas.SetActive(false);

        InitializeBGMManager();
        SetBGMForScene(SceneManager.GetActiveScene().name);
    }

    private void InitializeBGMManager()
    {
        bgmManager = BGMManager.instance;

        if (bgmManager == null)
        {
            bgmManager = new GameObject("BGMManager").AddComponent<BGMManager>();
            bgmManager.ResetInstance();
        }
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
        starsCollected++;

        if (starsCollected - 1 < estrellasActivas.Length)
        {
            estrellasActivas[starsCollected - 1].SetActive(true);
        }

        if (starsCollected >= 4)
        {
            _victoryCanvas.SetActive(true);
            if (bgmManager != null && victoryClip != null)
            {
                bgmManager.PlayBGM(victoryClip);
            }

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
        SetBGMForScene(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private void SetBGMForScene(string sceneName)
    {
        InitializeBGMManager();

        switch (sceneName)
        {
            case "Main Menu":
                Destroy(bgmManager.gameObject); // Reinicia el BGMManager para Main Menu
                bgmManager = null;
                InitializeBGMManager();
                bgmManager.PlayBGM(mainMenuBGM);
                break;
            case "Game Over":
                bgmManager.PlayBGM(gameOverBGM);
                break;
            case "Scene 1":
                bgmManager.PlayBGM(scene1BGM);
                break;
            default:
                bgmManager.StopBGM();
                break;
        }
    }
}
