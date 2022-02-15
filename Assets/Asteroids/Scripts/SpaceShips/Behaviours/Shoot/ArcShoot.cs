using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcShoot : NetworkBehaviour, IShootAble
{
    [SerializeField] private float fireCooldown = 1;

    [SerializeField] private float endAngle = 270;
    [SerializeField] private float startAngle = 90;
    [SerializeField] private int bulletsSpawn = 2;

    private float currentTimeShoot;

    private GenericFactory factory;
    public GameObject Gameobject => gameObject;


    private void Awake()
    {
        factory = GenericFactory.Instance;
    }

    [Server]
    public void SetShootValues(int _startValue, int _endValue, float increse)
    {
        startAngle = _startValue;
        endAngle = _endValue;
        bulletsSpawn = (int)increse;
    }

    public void Shoot(Vector2 position)
    {
        if (currentTimeShoot < fireCooldown)
        {
            return;
        }
        currentTimeShoot = 0;
        float playerAngle = Vector3Extension.Angle360(transform.up, Vector2.up);

        float angleStep = (endAngle - startAngle) / bulletsSpawn;
        float angle = startAngle - playerAngle;

        for (int i = 0; i < bulletsSpawn + 1; i++)
        {
            float xcomponent = Mathf.Cos((angle) * Mathf.PI / 180);
            float ycomponent = Mathf.Sin((angle) * Mathf.PI / 180);

            Vector2 bulletMoveVector = new Vector3(xcomponent, ycomponent, 0);
            Vector2 bullDir = (bulletMoveVector).normalized;

            BulletEntity bullet = factory.Create<BulletEntity>("ArcBullet");

            bullet?.Init(bullDir, transform.parent.gameObject, position, transform.parent.transform.rotation);
            angle += angleStep;
        }
    }

    public void UpdateBehaviour(float dt)
    {
        currentTimeShoot += dt;
    }
}
