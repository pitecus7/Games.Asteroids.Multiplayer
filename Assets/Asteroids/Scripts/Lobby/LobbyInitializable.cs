using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CentaurGames.Packages.Games.Core;

public class LobbyInitializable : MonoBehaviour, IInitializable
{
    public string SceneName => "Lobby";

    public void GetData(Action<object> callback)
    {
        callback?.Invoke("Nickname");
    }

    public void GetTestData(Action<object> callback)
    {
        callback?.Invoke("NicknameTest");
    }
}
