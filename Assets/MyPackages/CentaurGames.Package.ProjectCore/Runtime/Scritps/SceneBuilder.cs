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

        [SerializeField] private bool reflection;

        public void Init(List<IInitializable> initilizables, bool mode)
        {
            if (reflection)
            {
                initilizables.ForEach(initializer => this.initilizables.Add(initializer.SceneName, initializer));
            }
            else
            {
                for (int i = 0; i < GetComponents<IInitializable>().Length; i++)
                {
                    this.initilizables.Add(GetComponents<IInitializable>()[i].SceneName, GetComponents<IInitializable>()[i]);
                }
            }

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