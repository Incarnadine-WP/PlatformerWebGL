using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayerAttack() => Player.Instance.Attack();
    public void PlayRunAnim(float speed) => _animator.SetFloat("Speed", speed);
    public void PlayJumpAnim() => _animator.SetBool("IsAir", true);
    public void StopJumpAnim() => _animator.SetBool("IsAir", false);
    public void PlayAttackAnim() => _animator.SetTrigger("Attack");
    public void PlayLadderAnim(float speed) => _animator.SetFloat("Ladder", speed);
    public void IsGroundTrigger(bool isGround) => _animator.SetBool("IsLadder", isGround);
    public void PlayPlayerHurtAnim() => _animator.Play("Hurt");
    public void PlayDeathAnim() => _animator.Play("Death");
}
