using System;
using UnityEngine;

namespace CentaurGames.Packages.Games.Core
{
    public abstract class SceneController : MonoBehaviour
    {
        #region Fields
        public static SceneController Instance;
        protected GameManager GameManager => GameManager.Instance;

        public Action<string, object, Action<object>> SaveDataEvent;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Using to Init a Scene with the data neccessary
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="data">Content to show in the scene.</param>
        /// <param name="callback">Return Init proccess <Succes = fail or done> <code = number code proccess> <message: dev message to get more info></param>
        /// Codes:
        /// Success 200
        /// Data cant be null 301
        public abstract void Init(object data, Action<bool, uint, string> callback = null);

        protected virtual void Awake()
        {
            Instance = this;
        }
        #endregion Methods
    }
}
