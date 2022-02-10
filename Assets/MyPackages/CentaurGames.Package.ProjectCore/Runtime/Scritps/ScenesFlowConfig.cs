using CentaurGames.Packages.Games.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CentaurGames.Packages.Games.Core
{
    [CreateAssetMenu(fileName = "ScenesFlowConfig", menuName = "Game/Scenes Configuration", order = 1)]
    public class ScenesFlowConfig : ScriptableObject
    {
        public string loadingSceneName = "Loading";

        public string accessSceneName = "LogIn";

        public string debugCatcherSceneName;

        public bool testMode;

        public List<string> scenesNameFlow = new List<string>()
        {
            "Init",
            "Home",
            "RegularTournament",
            "LadderTournament",
            "Gameplay"
        };

        public List<string> InitializablesNames = new List<string>();

        private List<IInitializable> initializables = new List<IInitializable>();
        public List<IInitializable> Initializables
        {
            get
            {
                InitializablesNames.ForEach(initializable =>
                 {
                     Type type = Type.GetType(initializable);
                     if (type != null)
                     {
                         initializables.Add((IInitializable)Activator.CreateInstance(type));
                     }
                 });
                return initializables;
            }
        }
    }
}