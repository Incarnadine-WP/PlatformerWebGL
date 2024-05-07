using UnityEngine;

public class EnemyAttackState : StateMachineBehaviour
{
    [SerializeField] private float _attackRange = 1f;
   // [SerializeField] private float _distanceToChase = 6f;

    private Rigidbody2D _enemyRB;
    private EnemyPatrolLocation _enemyPatrol;
    private SkeletonEnemy _skeleton;
    private Transform _player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyRB = animator.GetComponentInParent<Rigidbody2D>();
        _skeleton = animator.GetComponentInParent<SkeletonEnemy>();
        _enemyPatrol = animator.GetComponentInParent<EnemyPatrolLocation>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_skeleton.IsDead)
        {
           // var distanceToPlayer = Vector2.Distance(_enemyRB.position, _player.position);

            if (_skeleton.IsChasing)
            {
                _skeleton.LookAtTarget(_player);

                float chaseSpeed = _skeleton.UnitStats.Speed * 1.5f * Time.fixedDeltaTime;

                Vector2 offsetForAttack = _skeleton.IsFlipped ? new Vector2(-0.75f, 0) : new Vector2(0.75f, 0);
                Vector2 target = new Vector2(_player.position.x, _enemyRB.position.y);
                Vector2 newPos = Vector2.MoveTowards(_enemyRB.position, target + offsetForAttack, chaseSpeed);
                _enemyRB.MovePosition(newPos);

                if (Vector2.Distance(_player.position, _enemyRB.position) <= _attackRange)
                    animator.SetTrigger("Attack");
            }
            else
                _enemyPatrol.MoveToPatrolPoints(_skeleton.UnitStats.Speed);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }


}
