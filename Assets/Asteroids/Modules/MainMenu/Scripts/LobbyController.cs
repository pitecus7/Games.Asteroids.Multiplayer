using System;
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

    private void MoveToGameplay(string obj)
    {

    }
}
