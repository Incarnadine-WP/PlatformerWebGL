using UnityEngine;
using DG.Tweening;

public class Chest : MonoBehaviour
{
    [SerializeField] private KeySymbol _keyItemUI;

    [SerializeField] private Animator _animator;

    private bool _playerNextToTheChest;
    private bool _isOpened;

    private void Start() => PlayerController.Instance.OnInteractAction += OpenChest;

    private void OnDisable() => PlayerController.Instance.OnInteractAction -= OpenChest;
   
    public bool IsOpened
    {
        get { return _isOpened; }
        set
        {
            _isOpened = value;
            _animator.SetBool("IsOpened", _isOpened);
        }
    }

    public void PlaySymbolAnim()
    {
        float jumpPower = 50f * Time.deltaTime;
        _keyItemUI.transform.DOJump(_keyItemUI.transform.position, jumpPower, 1, 1f);
        StartCoroutine(_keyItemUI.SymbolBoxColliderEnable());
    }

    public void Open() => IsOpened = true;
    public void Close() => IsOpened = false;

    private void OpenChest()
    {
        if (_playerNextToTheChest && !_isOpened)
        {
            Open();
            _keyItemUI.gameObject.SetActive(true);
            PlaySymbolAnim();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player != null)
                _playerNextToTheChest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player != null)
                _playerNextToTheChest = false;
        }
    }
}
