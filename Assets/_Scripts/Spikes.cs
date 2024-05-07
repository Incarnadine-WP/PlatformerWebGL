using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            player.PlayerRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            player.TakeDamage(_damage);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            player.PlayerRigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
    }
}
