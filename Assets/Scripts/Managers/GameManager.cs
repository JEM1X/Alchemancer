using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public int PlayerHealth;
    public int Score; // ����� ������ ���� �� �����
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
