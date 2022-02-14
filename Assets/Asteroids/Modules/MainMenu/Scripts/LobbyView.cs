using CentaurGames.Packages.Games.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour, ILobbyViewable
{
    [SerializeField] private InputField nicknameInputField;

    [SerializeField] private SettingsChannel settinsChannel;

    System.Action<string> ILobbyViewable.GoToGameplay { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        if (settinsChannel.nicknamePlayer.Length > 0)
        {
            nicknameInputField.text = settinsChannel.nicknamePlayer;
        }
        else
        {
            settinsChannel.nicknamePlayer = $"Player-{Random.Range(0, 99)}";
            nicknameInputField.text = settinsChannel.nicknamePlayer;
        }
    }

    public void SoloPlay()
    {
        settinsChannel.nicknamePlayer = nicknameInputField.text;
        settinsChannel.gameType = GameType.Solo;
        GoToGameplay();
    }

    public void HostPlay()
    {
        settinsChannel.nicknamePlayer = nicknameInputField.text;
        settinsChannel.gameType = GameType.Vs;
        settinsChannel.isHost = true;
        GoToGameplay();
    }

    public void ClientPlay()
    {
        settinsChannel.nicknamePlayer = nicknameInputField.text;
        settinsChannel.gameType = GameType.Vs;
        settinsChannel.isHost = false;
        GoToGameplay();
    }

    public void QuitAplication()
    {
        Application.Quit();
    }

    private void GoToGameplay()
    {
        //SceneManager.LoadScene("Gameplay");
        GameManager.Instance.FinishScene("Gameplay");
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }
}
