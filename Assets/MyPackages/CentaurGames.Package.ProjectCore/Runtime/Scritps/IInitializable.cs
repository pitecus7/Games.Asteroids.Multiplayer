using System;

namespace CentaurGames.Packages.Games.Core
{
    public interface IInitializable
    {
        public string SceneName { get; }
        public void GetData(Action<object> callback);
        public void GetTestData(Action<object> callback);
    }
}
