using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable, IAttacker
{
    [SerializeField] private Animator _animator;
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] private float _attackRadiusCircle = 0.5f;
    [SerializeField] protected LayerMask _targetForAttack;
    [SerializeField] protected UnitStats _unitStats;
    [SerializeField] private UnitHealthBar _healthBar;

    private Player _player;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    private bool _isDead = false;
    private int _currentHP;

    public UnityAction OnDeathAction;
    public UnityAction OnDamageAction;
    public bool IsFlipped = true;
    public bool IsInvulnerable = false;
    public bool IsDead => _isDead;
    public UnitStats UnitStats => _unitStats;

    private void Awake()
    {
        _currentHP = _unitStats.MaxHp;
        _player = (Player)FindObjectOfType(typeof(Player));
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _healthBar.SetMaxHealth(_unitStats.MaxHp);

        _player.OnDeathAction += () => _animator.Play("PlayerKilled");
    }

    private void OnDisable()
    {
        _player.OnDeathAction -= () => _animator.Play("PlayerKilled");
    }
        
    #region IDamagable
    public void TakeDamage(int damage)
    {
        if (IsInvulnerable)
            return;

        _currentHP -= damage;
        OnDamageAction?.Invoke();
        _healthBar.SetCurrentHealth(_currentHP);
        SoundManager.Instance.PlayHurtEnemySound(transform.position, 0.3f);
        //_animator.Play("Damage");

        if (_currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _isDead = true;
        _rigidbody2D.isKinematic = true;
        _collider2D.enabled = false;
        _healthBar.gameObject.SetActive(false);
        _animator.SetTrigger("Death");
        SoundManager.Instance.PlayDeathEnemySound(transform.position);
    }
    #endregion

    #region IAttacker
    public virtual void Attack()
    {
        var unitCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadiusCircle, _targetForAttack);
        if (unitCollider != null)
            unitCollider.GetComponent<Player>().TakeDamage(_unitStats.Damage);
    }
    #endregion


    public void LookAtTarget(Transform target)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > target.transform.position.x && IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            IsFlipped = false;
        }
        else if (transform.position.x < target.transform.position.x && !IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            IsFlipped = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadiusCircle);
    }

}
