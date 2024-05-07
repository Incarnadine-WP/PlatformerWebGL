using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable, IAttacker
{
    public static Player Instance;

    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius = 0.5f;
    [SerializeField] private UnitStats _unitStats;
    [SerializeField] private UnitHealthBar _playerHealthBar;
    [SerializeField] private float _invulnerableTime = 1f;

    private bool _isInvulnerable;
    private float _time;
    private int _keySymbolCount = 0;

    private Rigidbody2D _playerRB;

    public Rigidbody2D PlayerRigidbody => _playerRB;
    public BoxCollider2D BoxCollider2D => _boxCollider2D;
    public UnitStats PlayerStats => _unitStats;
    public int KeySymbolCount => _keySymbolCount;

    public UnityAction OnDamageAction;
    public UnityAction OnHealAction;
    public UnityAction OnDeathAction;
    public UnityAction OnTakeKeySymbol;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _playerRB = GetComponent<Rigidbody2D>();


    }

    private void Start()
    {
        InitPlayerHP();

        OnHealAction += () => _playerHealthBar.SetCurrentHealth(_unitStats.CurrentHp);
        OnDamageAction += () => _playerHealthBar.SetCurrentHealth(_unitStats.CurrentHp);
        OnTakeKeySymbol += AddKeySymbol;
    }

    private void OnDisable()
    {
        OnHealAction -= () => _playerHealthBar.SetCurrentHealth(_unitStats.CurrentHp);
        OnTakeKeySymbol -= AddKeySymbol;
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time > _invulnerableTime)
            InvulnerableIsOver();
    }

    public void InvulnerableIsOver() => _isInvulnerable = false;
    private void AddKeySymbol() => _keySymbolCount++;

    private void InitPlayerHP()
    {
        if (PlayerData.Instance.IsFirstScene)
        {
            _unitStats.CurrentHp = _unitStats.MaxHp;
            _playerHealthBar.SetMaxHealth(_unitStats.MaxHp);
            _playerHealthBar.SetCurrentHealth(_unitStats.CurrentHp);
        }
        else
        {
            _playerHealthBar.SetMaxHealth(_unitStats.MaxHp);
            _unitStats.CurrentHp = PlayerData.Instance.UpdatePlayerHP();
            _playerHealthBar.SetCurrentHealth(PlayerData.Instance.UpdatePlayerHP());
        }
    }

    #region IDamagable
    public void Die()
    {
        Debug.Log("You died!");
        OnDeathAction?.Invoke();
        PlayerController.Instance.DisablePlayerController();
    }

    public void TakeDamage(int dmg)
    {
        if (_isInvulnerable)
            return;

        _unitStats.CurrentHp -= dmg;
        _isInvulnerable = true;
        _time = 0f;

        if (_unitStats.CurrentHp > 0)
        {
            OnDamageAction?.Invoke();
            SoundManager.Instance.PlayHurtPlayerSound(transform.position, 0.8f);
        }

        else if (_unitStats.CurrentHp <= 0)
        {
            Die();
        }
    }
    #endregion

    #region IAttacker
    public void Attack()
    {
        var unitCollider = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius);

        foreach (var item in unitCollider)
        {
            if (item.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(_unitStats.Damage);
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }
}
