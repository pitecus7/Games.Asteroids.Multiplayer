using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataChannel", menuName = "ScriptableObjects/GameDataChannel", order = 3)]
public class GameDataChannel : ScriptableObject
{
    public Action<AsteroidEntity, SpaceshipEntity> OnAsteroidDestroyed;
    public Action<SpaceshipEntity, SpaceshipEntity> OnEnemyDestroyed;
    public void AsteroidDestroy(AsteroidEntity asteroid, SpaceshipEntity destroyer)
    {
        OnAsteroidDestroyed?.Invoke(asteroid, destroyer);
    }

    public void EnemyDestroy(SpaceshipEntity enemy, SpaceshipEntity destroyer)
    {
        OnEnemyDestroyed?.Invoke(enemy, destroyer);
    }
}
