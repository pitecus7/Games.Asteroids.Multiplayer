using CentaurGames.Packages.Games.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : SceneController
{
    public LobbyController lobbyController;
    public override void Init(object data, Action<bool, uint, string> callback = null)
    {
        callback?.Invoke(true, 200, "Ok.");
    }
}
