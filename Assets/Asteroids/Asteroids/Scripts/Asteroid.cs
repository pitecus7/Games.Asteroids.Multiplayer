using Mirror;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : NetworkBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;

    [SyncVar(hook = nameof(SetImage))] public int randomSprite = 0;
    public float size = 1f;
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 50f;
    public float maxLifetime = 30f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void SetImage(int oldValue, int newValue)
    {
        spriteRenderer.sprite = sprites[newValue];
    }

    void SetSize(float oldValue, float newValue)
    {
        transform.localScale = Vector3.one * newValue;
        rigidbody.mass = newValue;
    }


    [Server]
    public void SetValues(float _size)
    {
        // Assign random properties to make each asteroid feel unique
        randomSprite = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomSprite];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Set the scale and mass of the asteroid based on the assigned size so
        // the physics is more realistic
        transform.localScale = Vector3.one * _size;
        rigidbody.mass = _size;
        size = _size;
        SetClientValues(randomSprite, _size, transform.rotation);
        Invoke("HideAsteroid", maxLifetime);
    }

    [ClientRpc]
    private void SetClientValues(int randomSprite, float _size, Quaternion rotation)
    {
        spriteRenderer.sprite = sprites[randomSprite];
        transform.localScale = Vector3.one * _size;
        rigidbody.mass = _size;
        transform.rotation = rotation;
    }

    [Server]
    public void SetValues()
    {
        // Assign random properties to make each asteroid feel unique
        randomSprite = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[randomSprite];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Set the scale and mass of the asteroid based on the assigned size so
        // the physics is more realistic
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;
        SetClientValues(randomSprite, size, transform.rotation);
        Invoke("HideAsteroid", maxLifetime);
    }

    private void HideAsteroid()
    {
        CancelInvoke();
        if (spriteRenderer.isVisible)
        {
            Invoke("HideAsteroid", maxLifetime);
        }
        else
        {
            AsteroidSpawner.Instance.AddToPool(this);
        }
    }

    [ServerCallback]
    public void SetTrajectory(Vector2 direction)
    {
        // The asteroid only needs a force to be added once since they have no
        // drag to make them stop moving
        rigidbody.AddForce(direction * movementSpeed);
    }
    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameplayController.Instance.AsteroidDestroyed(this, collision.gameObject.GetComponent<Bullet>().PlayerShooter);
            if (!isServer)
                return;

            // Check if the asteroid is large enough to split in half
            // (both parts must be greater than the minimum size)
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            ShowExplosive();

            // Destroy the current asteroid since it is either replaced by two
            // new asteroids or small enough to be destroyed by the bullet

            AsteroidSpawner.Instance.AddToPool(this);

        }
    }
    [ServerCallback]
    private void CreateSplit()
    {
        // Set the new asteroid poistion to be the same as the current asteroid
        // but with a slight offset so they do not spawn inside each other
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        AsteroidSpawner.Instance.SpawnNewAsteriods(size, position, transform.rotation);
    }
    [ClientRpc]
    public void ShowExplosive()
    {
        GameplayController.Instance.ExplosionEffect.transform.position = transform.position;
        GameplayController.Instance.ExplosionEffect.Play();
    }

    [ClientRpc]
    public void HideToClients()
    {
        this.gameObject.SetActive(false);
    }
    [ClientRpc]
    public void ShowToClients(Vector2 position)
    {
        this.gameObject.transform.position = position;
        this.gameObject.SetActive(true);
    }
}
