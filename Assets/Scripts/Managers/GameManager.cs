using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public int PlayerHealth;
    public int Score; // чтобы писать очки за забег
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
