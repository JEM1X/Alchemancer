using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    
    public int TotalScore; // чтобы писать очки за забег
    protected override void Awake()
    {
        base.Awake();
        Enemy.OnScoreGain += AddScore;
        DontDestroyOnLoad(gameObject);
    }

    private void AddScore(int _score) 
    {
        TotalScore += _score;    
    }
}
