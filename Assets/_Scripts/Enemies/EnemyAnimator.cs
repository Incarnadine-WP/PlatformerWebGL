using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
   [SerializeField] private Enemy _enemy;

    public void EnemyAttack() => _enemy.Attack();
    public void SkeletonAttackSound() => SoundManager.Instance.PlaySkeletonAttackSound(_enemy.transform.position);
}
