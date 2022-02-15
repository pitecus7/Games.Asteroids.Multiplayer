using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletMovementBehaviour : MonoBehaviour, IBulletAble
{
    [SerializeField] private BulletActor bulletActor;
    [SerializeField] private float speed = 500f;

    public GameObject Gameobject => gameObject;

    private void Awake()
    {
        if (bulletActor == null)
        {
            bulletActor = GetComponentInParent<BulletActor>();
        }
    }

    public void SetTrajectory(Vector2 direction)
    {
        bulletActor?.RigidBody.RemoveVelocity();
        transform.parent.transform.eulerAngles = new Vector3(0, 0, direction.GetAngleFromVector() - 90);
        bulletActor?.RigidBody.AddForce(direction * speed);
    }

    public void UpdateBehaviour(float dt)
    {
    }
}
