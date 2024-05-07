using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FireEarth : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;

    private float _delay = 0.65f;

    private void OnEnable()
    {
        transform.DOLocalMoveY(_endPosition.position.y, _delay);
        SoundManager.Instance.PlaySpellFireEarthSound(transform.position);
    }

    public void StopFireEarth() => StartCoroutine(StopCastFireEarth());
    private IEnumerator StopCastFireEarth()
    {
        var delay = new WaitForSeconds(_delay);
        transform.DOLocalMoveY(_startPosition.position.y, _delay);
        yield return delay;
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
            player.PlayerRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
            player.TakeDamage(_damage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
            player.PlayerRigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
    }
}
