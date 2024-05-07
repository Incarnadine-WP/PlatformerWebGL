using UnityEngine;

public class BossStateMachine : StateMachineBehaviour
{
    private float _delayToAction;
    private float _randomSpell;
    protected EnemyBoss _boss;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss = animator.GetComponentInParent<EnemyBoss>();
        _boss.IsAttakTime = false;

        if (_boss.IsFlipped)
            _boss.LookAtTarget(_boss.PointsToCharge[0].transform);
        else
            _boss.LookAtTarget(_boss.PointsToCharge[1].transform);

        _randomSpell = Random.Range(1, 4);
        _delayToAction = 2.5f;
        animator.SetInteger("Blend", 0);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delayToAction >= 0)
            _delayToAction -= Time.deltaTime;

        if (_delayToAction < 0)
        {
            animator.SetFloat("RandomSpell", _randomSpell);
            animator.SetInteger("Blend", 1);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
