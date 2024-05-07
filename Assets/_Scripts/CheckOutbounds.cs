using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutbounds : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            player.TakeDamage(player.PlayerStats.MaxHp);
    }
}
