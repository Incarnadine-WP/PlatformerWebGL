using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BatAttackState : StateMachineBehaviour
{
    private BatEnemy _bat;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponentInParent<BatEnemy>();

        if (!_bat.IsDead)
        {
            float correctAnimSpeed = 0.2f;

            if (_bat.transform.position == _bat.StartPosition.position)
                _bat.transform.DOMove(_bat.EndPosition.position, stateInfo.length + correctAnimSpeed).SetEase(Ease.InOutBack);
            else
                _bat.transform.DOMove(_bat.StartPosition.position, stateInfo.length + correctAnimSpeed).SetEase(Ease.InOutBack);

            SoundManager.Instance.PlayBatChargeSound(_bat.transform.position);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat.Attack();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
