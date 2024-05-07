using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private int _playerHP;

    public static PlayerData Instance;

    public bool IsFirstScene = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _playerHP = Player.Instance.PlayerStats.MaxHp;
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayerHP(int HP)
    {
        _playerHP = HP;
        IsFirstScene = false;
    }

    public int UpdatePlayerHP()
    {
        return _playerHP;
    }
}
