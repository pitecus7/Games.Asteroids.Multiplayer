using System;
using System.Collections.Generic;
using UnityEngine;

namespace CentaurGames.Packages.Games.Core
{
    public static class GameDataController
    {
        private static Dictionary<string, Action<object, Action<object>>> Actions = new Dictionary<string, Action<object, Action<object>>>();

        public static void Init()
        {
            if (SceneController.Instance != null)
            {
                SceneController.Instance.SaveDataEvent -= ReadEvent;
                SceneController.Instance.SaveDataEvent += ReadEvent;
            }
        }

        public static void ReadEvent(string Process, object data, Action<object> callback)
        {
            if (Actions.ContainsKey(Process))
            {
                Actions[Process].Invoke(data, callback);
            }
            else
            {
                Debug.Log("Huston we have a problem from GameDataController");
            }
        }

        public static void AddListener(string methodname, Action<object, Action<object>> action)
        {
            Actions.Add(methodname, action);
        }

        public static void RemoveListener(string methodname)
        {
            Actions.Remove(methodname);
        }
    }
}
