using Mirror;
using UnityEngine;

public class GameplayStatus : NetworkBehaviour
{
    public static GameplayStatus Instance;

    [SerializeField] private GameDataChannel gameDataChannel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [ClientRpc]
    public void FinishMatch()
    {
        DoWhatUNeed();
    }

    public void DoWhatUNeed()
    {
        gameDataChannel?.FinishMatch();
    }
}
