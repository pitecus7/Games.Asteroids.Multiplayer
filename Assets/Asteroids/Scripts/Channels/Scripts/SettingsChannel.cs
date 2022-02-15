using UnityEngine;

[CreateAssetMenu(fileName = "SettingsChannel", menuName = "ScriptableObjects/SettingsChannel", order = 2)]
public class SettingsChannel : ScriptableObject
{
    public string nicknamePlayer;
    public GameType gameType;
    public bool isHost;
    public string networkAddress;
    public ushort port;
}

public enum GameType
{
    Solo,
    Vs
}