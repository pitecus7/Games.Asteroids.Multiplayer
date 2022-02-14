using CentaurGames.Packages.Games.Core;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameplaySceneController : SceneController
{
    [SerializeField] private GameplayControllerCore gameplayController;
    [SerializeField] private AsteroidNetworkManager networkManager;

    private SettingsChannel gameplayData;
    public override void Init(object data, Action<bool, uint, string> callback = null)
    {
        if (networkManager == null)
            networkManager = FindObjectOfType<AsteroidNetworkManager>();

        if (data.GetType() == typeof(SettingsChannel))
        {
            gameplayData = (SettingsChannel)data;
            networkManager.Init(gameplayData.gameType == GameType.Solo);

            SceneManager.LoadSceneAsync("ClassicGameplay", LoadSceneMode.Additive).completed += OnGameplayControllerLoaded;

            callback?.Invoke(true, 200, "Ok");
        }
    }

    private void OnGameplayControllerLoaded(AsyncOperation obj)
    {
        gameplayController = GameplayControllerCore.Instance;

        gameplayController.Init(gameplayData, () =>
        {
            StartConnection();
        });
    }

    private void StartConnection()
    {
        if (gameplayData.gameType == GameType.Solo || (gameplayData.gameType != GameType.Solo && gameplayData.isHost))
        {
            networkManager.StartHost();
        }
        else
        {
            networkManager.StartClient();
        }
    }

    public void BackToLobby()
    {
        networkManager.StopHost();
        SceneManager.UnloadSceneAsync("ClassicGameplay").completed += (asyncdata) =>
        {
            GameManager.Instance.FinishScene("Lobby");
        };
    }
}
