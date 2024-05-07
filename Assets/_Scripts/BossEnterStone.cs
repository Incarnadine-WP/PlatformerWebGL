using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class BossEnterStone : MonoBehaviour
{
    [SerializeField] private Color _enterColor;
    [SerializeField] private SpriteRenderer _symbol;
    [SerializeField] private TextMeshProUGUI _enterTheDoor;

    private Animator _animator;
    private bool _playerNextToBossStone;
    private string _openDoorInfo = "To enter the boss you need to collect 3 Symbol";

    public static BossEnterStone Instance;
    public UnityAction OnEnterBossScene;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        PlayerController.Instance.OnInteractAction += () => StartCoroutine(EnterBossScene());
        OnEnterBossScene += GameManager.Instance.StartDarkPanel;

        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    private void OnDisable()
    {
        PlayerController.Instance.OnInteractAction -= () => StartCoroutine(EnterBossScene());
        OnEnterBossScene -= GameManager.Instance.StartDarkPanel;

        DOTween.Kill(transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player != null)
                _playerNextToBossStone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player != null)
                _playerNextToBossStone = false;
        }
    }

    private IEnumerator EnterBossScene()
    {
        var delay = new WaitForSeconds(2f);
        if (_playerNextToBossStone && Player.Instance.KeySymbolCount >= 3)
        {
            _symbol.DOColor(_enterColor, 0.4f);
            PlayerController.Instance.DisablePlayerController();
            _animator.enabled = true;
            OnEnterBossScene?.Invoke();
            yield return delay;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData.Instance.SavePlayerHP(Player.Instance.PlayerStats.CurrentHp);
        }
        else if (_playerNextToBossStone)
        {
            _enterTheDoor.gameObject.SetActive(true);
            _enterTheDoor.text = _openDoorInfo;
            yield return delay;
            _enterTheDoor.gameObject.SetActive(false);
        }
    }
}
