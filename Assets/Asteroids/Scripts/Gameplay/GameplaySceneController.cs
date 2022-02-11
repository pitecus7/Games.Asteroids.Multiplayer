using CentaurGames.Packages.Games.Core;
using UnityEngine;
using System;

public class GameplaySceneController : SceneController
{
    public override void Init(object data, Action<bool, uint, string> callback = null)
    {
        Debug.Log("Gameplay Started.");
    }
}
