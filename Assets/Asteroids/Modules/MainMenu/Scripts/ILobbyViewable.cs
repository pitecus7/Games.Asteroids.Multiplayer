using System;

public interface ILobbyViewable
{
    public Action<string> GoToGameplay { get; set; }
    public void Init();
}
