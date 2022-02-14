using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataChannel", menuName = "ScriptableObjects/GameDataChannel", order = 3)]
public class GameDataChannel : ScriptableObject
{
    public Action<AsteroidEntity, SpaceshipEntity> OnAsteroidDestroyed;
    public Action<SpaceshipEntity, SpaceshipEntity> OnEnemyDestroyed;
    public Action OnFinishMatch;

    public void AsteroidDestroy(AsteroidEntity asteroid, SpaceshipEntity destroyer)
    {
        OnAsteroidDestroyed?.Invoke(asteroid, destroyer);
    }

    public void EnemyDestroy(SpaceshipEntity enemy, SpaceshipEntity destroyer)
    {
        OnEnemyDestroyed?.Invoke(enemy, destroyer);
    }

    public void FinishMatch()
    {
        OnFinishMatch?.Invoke();
    }
}
