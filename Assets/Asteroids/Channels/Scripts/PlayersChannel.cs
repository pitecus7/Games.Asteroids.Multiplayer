using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersChannel", menuName = "ScriptableObjects/PlayersChannel", order = 1)]
public class PlayersChannel : ScriptableObject
{
    public Action<Player> OnPlayerJoined;

    public void AddPlayer(Player player)
    {

        OnPlayerJoined?.Invoke(player);
    }
}
