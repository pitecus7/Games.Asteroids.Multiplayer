using System;
using CentaurGames.Packages.Games.Core;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    private ILobbyViewable lobbyView;

    public void Init()
    {
        if (TryGetComponent(out lobbyView))
        {
            lobbyView.Init();
            lobbyView.GoToGameplay += MoveToGameplay;
        }
    }

    private void MoveToGameplay(string sceneName)
    {
        GameManager.Instance.FinishScene(sceneName);
    }

    private void OnDestroy()
    {
        lobbyView.GoToGameplay -= MoveToGameplay;
    }
}
