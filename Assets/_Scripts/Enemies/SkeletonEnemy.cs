using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    public bool IsChasing;

    public override void Attack()
    {
        base.Attack();
        SoundManager.Instance.PlaySkeletonAttackSound(transform.position);
    }
}
