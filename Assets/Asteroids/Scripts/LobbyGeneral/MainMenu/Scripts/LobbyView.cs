using CentaurGames.Packages.Games.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour, ILobbyViewable
{
    [SerializeField] private InputField nicknameInputField;

    [SerializeField] private SettingsChannel settinsChannel;

    public Action<string> GoToGameplay { get; set; }

    private void Start()
    {
        if (settinsChannel.nicknamePlayer.Length > 0)
        {
            nicknameInputField.text = settinsChannel.nicknamePlayer;
        }
        else
        {
            settinsChannel.nicknamePlayer = $"Player-{UnityEngine.Random.Range(0, 99)}";
            nicknameInputField.text = settinsChannel.nicknamePlayer;
        }
    }
    public void Init()
    {
    }

    public void SoloPlay()
    {
        settinsChannel.nicknamePlayer = nicknameInputField.text;
        settinsChannel.gameType = GameType.Solo;
        GoToGameplay?.Invoke("Gameplay");
    }

    public void HostPlay()
    {
        settinsChannel.nicknamePlayer = nicknameInputField.text;
        settinsChannel.gameType = GameType.Vs;
        settinsChannel.isHost = true;
        GoToGameplay?.Invoke("Gameplay");
    }

    public void ClientPlay()
    {
        settinsChannel.nicknamePlayer = nicknameInputField.text;
        settinsChannel.gameType = GameType.Vs;
        settinsChannel.isHost = false;
        GoToGameplay?.Invoke("Gameplay");
    }

    public void QuitAplication()
    {
        Application.Quit();
    }
}
