using Mirror;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public int Score { get => score; }

    [SyncVar(hook = nameof(ScoreChanged)), SerializeField] private int score = 0;
    
    [Header("Events"), Space]
    public IntEvent OnScoreChange;

    private void ScoreChanged(int oldValue, int newValue)
    {
        OnScoreChange?.Invoke(newValue);
    }

    [Server]
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    [Server]
    public void RemoveScore(int scoreToRemove)
    {
        if (score - scoreToRemove >= 0)
            score -= scoreToRemove;
        else
            score = 0;
    }
}
