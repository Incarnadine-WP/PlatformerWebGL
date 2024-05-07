using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Spell _spell;

    private void Start()
    {
        Transform player = Player.Instance.transform;
        float offsetPosZ = 90f;
        _spell.SetDirectionAndRotationToTarget(player, transform, -offsetPosZ);

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        var delay = new WaitForSeconds(1f);

        yield return delay;

        _spell.gameObject.SetActive(true);

        Destroy(gameObject);
    }
}
