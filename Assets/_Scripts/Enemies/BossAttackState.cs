using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossAttackState : BossStateMachine
{
    private bool _isAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss = animator.GetComponentInParent<EnemyBoss>();

        var animClip = animator.GetNextAnimatorClipInfo(0)[0].clip.name;
        if (animClip == "Attack")
            _isAttack = true;

        if (_isAttack)
        {
            float correctAnimSpeed = 0.3f;
            if (_boss.transform.position == _boss.PointsToCharge[1].position)
                _boss.transform.DOMove(_boss.PointsToCharge[0].position, stateInfo.length - correctAnimSpeed).SetEase(Ease.InBack);
            else
                _boss.transform.DOMove(_boss.PointsToCharge[1].position, stateInfo.length - correctAnimSpeed).SetEase(Ease.InBack);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isAttack && _boss.IsAttakTime)
            _boss.Attack();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _isAttack = false;
        _boss.IsAttakTime = false;
    }

}
