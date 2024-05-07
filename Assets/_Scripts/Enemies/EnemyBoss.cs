using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBoss : Enemy
{
    [SerializeField] private Vector2 _attackRadius;
    [SerializeField] private Transform[] _pointsToCharge;
    [SerializeField] private Transform[] _pointsForSpells;
    [SerializeField] private Transform[] _pointsForSpikes;

    [Header("Skills")] [Space(5)]
    [SerializeField] private EnemyShooting _portalPrefab;
    [SerializeField] private FireEarth _fireEarthPrefab;

    private SpriteRenderer _sprite;

    public UnityAction OnFireEarthStop;

    public Transform[] PointsToCharge => _pointsToCharge;
    public bool IsAttakTime;

    private void Start()
    {
         _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        OnDamageAction += StartDamageAnim;
        OnFireEarthStop += _fireEarthPrefab.StopFireEarth;
    }

    private void OnDisable()
    {
        OnDamageAction -= StartDamageAnim;
        OnFireEarthStop -= _fireEarthPrefab.StopFireEarth;
    }

    public override void Attack()
    {
        var unitCollider = Physics2D.OverlapCapsule(_attackPoint.position, _attackRadius, CapsuleDirection2D.Horizontal, 0f, _targetForAttack);
        if (unitCollider != null)
            unitCollider.GetComponent<Player>().TakeDamage(_unitStats.Damage);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.OnWinGame?.Invoke();
    }

    public void CastPortalSpell()
    {
        int randomIndex = Random.Range(0, 8);
        Instantiate(_portalPrefab, _pointsForSpells[randomIndex]);
    }

    private IEnumerator DamageAnim()
    {
        var delay = new WaitForSeconds(0.15f);
        Color startColor = _sprite.color;
        _sprite.color = new Color(255f, 0, 0, 150f);
        yield return delay;
        _sprite.color = startColor;
    }

    public void CastFireEarth() => _fireEarthPrefab.gameObject.SetActive(true);
    public void StopCastingFireEarth() => OnFireEarthStop?.Invoke();
    private void StartDamageAnim() => StartCoroutine(DamageAnim());

}
