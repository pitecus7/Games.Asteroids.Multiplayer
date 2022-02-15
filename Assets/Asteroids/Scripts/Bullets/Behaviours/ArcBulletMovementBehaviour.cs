using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBulletMovementBehaviour : MonoBehaviour, IBulletAble
{
    public GameObject Gameobject => gameObject;

    [SerializeField] float speed;

    private Vector3 direction;

    public void SetTrajectory(Vector2 _direction)
    {
        direction = _direction;
        transform.parent.transform.eulerAngles = new Vector3(0, 0, _direction.GetAngleFromVector() - 90);
    }

    public void UpdateBehaviour(float dt)
    {
        transform.parent.transform.position += direction * speed * dt;
    }
}
