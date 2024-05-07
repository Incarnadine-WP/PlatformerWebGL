using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;
    private AudioSource _audioSource;

    public static SoundManager Instance;

    private float _soundVolume = 1f;
    private float _musicVolume;
    private bool _isMusicPlay = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _audioSource = GetComponent<AudioSource>();
        _musicVolume = _audioSource.volume;
    }

    private void PlaySound(AudioClip audioClip, Vector2 position, float volume = 0.5f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * _soundVolume);
    }

    public void PlayMusic()
    {
        if (_isMusicPlay)
        {
            _isMusicPlay = false;
            _audioSource.volume = 0f;
            _soundVolume = 0f;
        }
        else
        {
            _isMusicPlay = true;
            _audioSource.volume = _musicVolume;
            _soundVolume = 1f;
        }
    }

    public void PlayFootStep(Vector3 position, float volume = 0.6f)
    {
        PlaySound(_audioClipRefsSO.FootStep, position, volume);
    }

    public void PlayJumpSound(Vector3 position, float volume = 0.6f)
    {
        PlaySound(_audioClipRefsSO.Jump, position, volume);
    }

    public void PlayHealSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.Heal, position, volume);
    }

    public void PlayTakeSymbolSound(Vector3 position, float volume = 0.6f)
    {
        PlaySound(_audioClipRefsSO.TakeSymbolKey, position, volume);
    }

    public void PlayHurtEnemySound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.HurtEnemy, position, volume);
    }

    public void PlayDeathEnemySound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.EnemyDeath, position, volume);
    }

    public void PlayPlayerAttackSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.PlayerAttack, position, volume);
    }

    public void PlaySkeletonAttackSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.SkeletonAttack, position, volume);
    }

    public void PlayHurtPlayerSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.HurtPlayer, position, volume);
    }

    public void PlayBatChargeSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.BatCharge, position, volume);
    }

    public void PlayBossChargeSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.BossCharge, position, volume);
    }

    public void PlaySpellFireEarthSound(Vector3 position, float volume =0.6f)
    {
        PlaySound(_audioClipRefsSO.SpellFireEarth, position, volume);
    }

    public void PlaySpellFireBallFlySound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.SpellFireBallFly, position, volume);
    }

    public void PlaySpellFireBallExplosionSound(Vector3 position, float volume = 0.5f)
    {
        PlaySound(_audioClipRefsSO.SpellFireBallExplosion, position, volume);
    }
}
