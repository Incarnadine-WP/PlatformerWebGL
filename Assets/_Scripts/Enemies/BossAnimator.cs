using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    [SerializeField] private EnemyBoss _boss;

    public void BossAttack() => _boss.IsAttakTime = true;
    public void BossCastSpell() => _boss.CastPortalSpell();
    public void BossCastFireEarth() => _boss.CastFireEarth();
    public void BossStopFireEarth() => _boss.StopCastingFireEarth();

    public void BossChargeSound()
    {
        SoundManager.Instance.PlayBossChargeSound(_boss.transform.position);
    }
}
