using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CentaurGames.Packages.Games.Core
{
    public class SceneBuilder : MonoBehaviour
    {
        private Dictionary<string, IInitializable> initilizables = new Dictionary<string, IInitializable>();

        private bool isTest;

        public void Init(List<IInitializable> initilizables, bool mode)
        {
            initilizables.ForEach(initializer => this.initilizables.Add(initializer.SceneName, initializer));

            Mode(mode);
        }

        public void Mode(bool mode)
        {
            isTest = mode;
        }

        public void InitScene(Scene scene, Action<bool, uint, string> callback = null)
        {
            GameDataController.Init();
            if (initilizables.ContainsKey(scene.name))
            {
                if (!isTest)
                {
                    initilizables[scene.name].GetData((data) =>
                    {
                        SceneController.Instance?.Init(data, callback);
                    });
                }
                else
                {
                    initilizables[scene.name].GetTestData((data) =>
                    {
                        SceneController.Instance?.Init(data, callback);
                    });
                }
            }
            else
            {
                SceneController.Instance?.Init(0, callback);
            }
        }
    }
}