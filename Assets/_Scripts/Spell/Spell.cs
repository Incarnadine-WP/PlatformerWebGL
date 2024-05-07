using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Spell : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _force;
    [SerializeField] private GameObject _explosionEff;

    private Rigidbody2D _rigidbody2D;

    private void OnEnable() => transform.parent = null;

    private void Start()
    {
        Transform player = Player.Instance.transform;
        float offsetPosZ = 90f;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = SetDirectionAndRotationToTarget(player, transform, offsetPosZ) * _force;
        SoundManager.Instance.PlaySpellFireBallFlySound(transform.position);
    }

    public Vector3 SetDirectionAndRotationToTarget(Transform targetTransform, Transform currentTransform, float offsetRotationZ)
    {
        Vector2 direction = targetTransform.position - currentTransform.position;
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentTransform.rotation = Quaternion.Euler(0, 0, rotation - offsetRotationZ);

        return direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
            Explosion();
        }
        else if (collision.CompareTag("Ground"))
            Explosion();
    }

    private void Explosion()
    {
        _explosionEff.transform.parent = null;
        _explosionEff.SetActive(true);
        SoundManager.Instance.PlaySpellFireBallExplosionSound(_explosionEff.transform.position);
        Destroy(gameObject);
    }
}
