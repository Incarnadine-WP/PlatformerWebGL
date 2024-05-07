using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Button _restart;
    [SerializeField] private Button _continue;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _victoryPanel;

    [SerializeField] private Image _darkPanel;

    public static GameManager Instance;
    public UnityAction OnWinGame;

    private static bool _isPaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _restart.onClick.AddListener(RestartScene);
        _continue.onClick.AddListener(GamePause);

        OnWinGame += () => StartCoroutine(ShowVictoryPanel());
        Player.Instance.OnDeathAction += () => StartCoroutine(ShowRestartPanel());
        PlayerController.Instance.OnPauseGame += GamePause;
    }

    private void OnDisable()
    {
        OnWinGame -= () => StartCoroutine(ShowVictoryPanel());
        Player.Instance.OnDeathAction -= () => StartCoroutine(ShowRestartPanel());
        PlayerController.Instance.OnPauseGame -= GamePause;

        _restart.onClick.RemoveListener(RestartScene);
        _continue.onClick.RemoveListener(GamePause);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        PlayerData.Instance.SavePlayerHP(Player.Instance.PlayerStats.MaxHp);
        DOTween.KillAll();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        PlayerData.Instance.SavePlayerHP(Player.Instance.PlayerStats.MaxHp);
        DOTween.KillAll();
    }

    private IEnumerator ShowRestartPanel()
    {
        var delay = new WaitForSeconds(1f);
        Time.timeScale = 0.65f;
        yield return delay;
        _menuPanel.SetActive(true);
    }

    private IEnumerator ShowVictoryPanel()
    {
        var delay = new WaitForSeconds(1f);
        Time.timeScale = 0.65f;
        yield return delay;
        _victoryPanel.SetActive(true);
    }

    private void GamePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            PlayerController.Instance.UnsubscribePlayerController();
            _menuPanel.SetActive(true);
            _isPaused = true;
            _continue.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            PlayerController.Instance.SubscribePlayerController();
            _menuPanel.SetActive(false);
            _continue.gameObject.SetActive(false);
            _isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void StartDarkPanel()
    {
        Color color = Color.black;
        _darkPanel.DOColor(color, 1.9f);
    }
}
