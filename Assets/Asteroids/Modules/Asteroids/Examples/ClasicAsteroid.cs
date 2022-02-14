
using Mirror;
using UnityEngine;

public class ClasicAsteroid : AsteroidEntity
{
    [SerializeField] private IMoveable movementBehaviour;

    [SerializeField] private float maxLifetime = 30f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (movementBehaviour == null)
        {
            movementBehaviour = GetComponentInChildren<IMoveable>();
        }
    }

    public override void Init(object data)
    {
        gameObject.SetActive(true);
        ShowToClients(transform.position);
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        if (data == null)
        {
            movementBehaviour.SetTrajectory(Random.insideUnitCircle.normalized);
        }
        else
        {
            Vector2 trajectory = (Vector2)data;
            movementBehaviour.SetTrajectory(trajectory);
        }
        Invoke(nameof(ValidateLife), maxLifetime);
    }

    private void ValidateLife()
    {
        CancelInvoke();
        if (spriteRenderer.isVisible)
        {
            Invoke(nameof(ValidateLife), maxLifetime);
        }
        else
        {
            HideToClients();
            OnAddToPool?.Invoke(Id, gameObject);
        }
    }

    [ServerCallback]
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        ShowExplosive();
        CancelInvoke();
        HideToClients();
        OnAddToPool?.Invoke(Id, gameObject);
    }

    [ClientRpc]
    public void ShowExplosive()
    {
        VfxController.Instance?.Explote(transform.position);
    }

    [ClientRpc]
    public void HideToClients()
    {
        gameObject.SetActive(false);
    }

    [ClientRpc]
    public void ShowToClients(Vector2 position)
    {
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }
}
