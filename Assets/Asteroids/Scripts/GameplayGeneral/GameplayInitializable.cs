using CentaurGames.Packages.Games.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInitializable : MonoBehaviour, IInitializable
{
    [SerializeField] private SettingsChannel settingsChannel;
    public string SceneName => "Gameplay";

    public void GetData(Action<object> callback)
    {
        callback?.Invoke(settingsChannel);
    }

    public void GetTestData(Action<object> callback)
    {
        callback?.Invoke(settingsChannel);
    }
}
