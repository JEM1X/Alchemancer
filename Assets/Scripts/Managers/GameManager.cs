using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int TotalScore;


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
