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
            networkManager.Init(gameplayData.gameType == GameType.Solo, gameplayData.networkAddress, gameplayData.port);

            if (gameplayData.gameType != GameType.Solo)
            {
                SceneManager.LoadSceneAsync("ClassicGameplay", LoadSceneMode.Additive).completed += OnGameplayControllerLoaded;
            }
            else if (gameplayData.gameType == GameType.Solo)
            {
                SceneManager.LoadSceneAsync("SoloGameplay", LoadSceneMode.Additive).completed += OnGameplayControllerLoaded;
            }

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

        if (gameplayData.gameType != GameType.Solo)
        {
            SceneManager.UnloadSceneAsync("ClassicGameplay").completed += (asyncdata) =>
            {
                GameManager.Instance.FinishScene("Lobby");
            };
        }
        else if (gameplayData.gameType == GameType.Solo)
        {
            SceneManager.UnloadSceneAsync("SoloGameplay").completed += (asyncdata) =>
            {
                GameManager.Instance.FinishScene("Lobby");
            };
        }


    }
}
