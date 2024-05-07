using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class HealPot : MonoBehaviour
{
    [SerializeField] private int _heal;
    [SerializeField] private ParticleSystem _vfxPrefab;

    private void Start()
    {
        transform.DOMoveY(transform.position.y + 0.2f, 0.4f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.PlayerStats.CurrentHp += _heal;

            if (player.PlayerStats.CurrentHp > player.PlayerStats.MaxHp)
                player.PlayerStats.CurrentHp = player.PlayerStats.MaxHp;

            player.OnHealAction?.Invoke();
            PlayVFX();
            SoundManager.Instance.PlayHealSound(player.transform.position);

            Destroy(gameObject);
        }
    }

    private void PlayVFX()
    {
        _vfxPrefab.gameObject.transform.parent = null;
        _vfxPrefab.Play();
        Destroy(_vfxPrefab.gameObject, _vfxPrefab.main.duration);
    }
}
