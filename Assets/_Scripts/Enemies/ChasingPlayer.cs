using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChasingPlayer : MonoBehaviour
{
    [SerializeField] private SkeletonEnemy _skeletonEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _skeletonEnemy.IsChasing = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _skeletonEnemy.IsChasing = false;
    }
}
