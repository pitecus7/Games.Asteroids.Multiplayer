using UnityEngine;

public interface IUpdateable
{
    public GameObject Gameobject { get; }
    void UpdateBehaviour(float dt);
}