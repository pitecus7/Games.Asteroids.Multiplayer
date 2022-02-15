using UnityEngine;
/// <summary>
/// Used to add Strategy pattern and State pattern
/// </summary>
public interface IShootAble : IUpdateable
{
    public void Shoot(Vector2 position);
}
