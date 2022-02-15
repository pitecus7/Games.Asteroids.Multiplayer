using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxController : MonoBehaviour
{
    public static VfxController Instance;

    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] public ParticleSystem ExplosionEffect => explosionEffect;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Explote(Vector2 position)
    {
        explosionEffect.transform.position = position;
        explosionEffect.Play();
    }
}
