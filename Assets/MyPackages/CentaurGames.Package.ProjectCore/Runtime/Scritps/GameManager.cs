using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CentaurGames.Packages.Games.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private SceneBuilder sceneBuilder;

        [SerializeField] private ScenesFlowConfig scenesFlowConfig;

        [SerializeField] private string currentScene;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInit()
        {
            foreach (GameObject commonObject in Resources.LoadAll("Common", typeof(GameObject)))
            {
                Instantiate(commonObject);
            }

            if (Instance != null)
            {
                SceneManager.sceneLoaded += Instance.OnSceneLoad;
            }
        }

        private void Awake()
        {
            if (!scenesFlowConfig)
                scenesFlowConfig = Resources.Load<ScenesFlowConfig>("Config/ScenesFlowConfig");

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
            if (!sceneBuilder)
                sceneBuilder = GetComponentInChildren<SceneBuilder>();

            if (sceneBuilder && scenesFlowConfig != null)
            {
                sceneBuilder.Init(scenesFlowConfig.Initializables, scenesFlowConfig.testMode);
            }

            DontDestroyOnLoad(this);
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene.name;
            sceneBuilder.InitScene(scene, OnInitSceneResponse);
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        public void FinishScene(string nextScene = null)
        {
            if (string.IsNullOrEmpty(nextScene))
            {
                nextScene = scenesFlowConfig?.scenesNameFlow[scenesFlowConfig.scenesNameFlow.FindIndex(x => x == this.currentScene) + 1];
            }

            LoadLoadingScene(success =>
            {
                UnloadScene(currentScene, success =>
                    {
                        LoadScene(nextScene, success =>
                            {
                                if (success)
                                {
                                    currentScene = nextScene;
                                    sceneBuilder.InitScene(SceneManager.GetSceneByName(currentScene), OnInitSceneResponse);
                                }
                                else
                                {
                                    LoadScene(scenesFlowConfig.debugCatcherSceneName);
                                }
                            });
                    });
            });
        }

        private void OnInitSceneResponse(bool success, uint code, string message)
        {
            if (success)
            {
                if (scenesFlowConfig != null && !string.IsNullOrEmpty(scenesFlowConfig.loadingSceneName) && Application.CanStreamedLevelBeLoaded(scenesFlowConfig.loadingSceneName))
                {
                    UnloadScene(scenesFlowConfig.loadingSceneName);
                }
            }
            else
            {
                //TODO:
                Debug.Log("Process error");
            }
        }

        private void LoadLoadingScene(Action<bool> callback = null)
        {
            if (scenesFlowConfig != null && !string.IsNullOrEmpty(scenesFlowConfig.loadingSceneName) && Application.CanStreamedLevelBeLoaded(scenesFlowConfig.loadingSceneName))
            {
                SceneManager.LoadSceneAsync(scenesFlowConfig?.loadingSceneName, LoadSceneMode.Additive).completed += (asyncOperation) => callback?.Invoke(true);
            }
            else
            {
                Debug.LogWarning("Loading scene can't be load, Configured it before to start");
                //TODO: 
                callback?.Invoke(false);
            }
        }

        private void LoadScene(string sceneName, Action<bool> callback = null)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName) && Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (asyncOperation) => callback?.Invoke(true);
            }
            else
            {
                Debug.LogError($"Scene {sceneName} can't be load, Validate if not added in Build settings");
                callback?.Invoke(false);
            }
        }

        private void UnloadScene(string sceneName, Action<bool> callback = null)
        {
            if (SceneManager.sceneCount > 1 && Application.CanStreamedLevelBeLoaded(sceneName) && SceneManager.GetSceneByName(sceneName).IsValid())
            {
                SceneManager.UnloadSceneAsync(sceneName).completed += (asyncOperation) => callback?.Invoke(true);
            }
            else
            {
                Debug.LogError($"Scene {sceneName} can't be unload");
                callback?.Invoke(false);
                if (scenesFlowConfig != null && !scenesFlowConfig.testMode)
                {
                    Debug.Log("Process error");
                }
            }
        }
    }
}
