using System.Collections;
using UnityEngine;

public class CannonProjectile : Projectile
{
    [SerializeField, Min(0f)] private float _lifetime; // Endless life insurance
    [SerializeField] private ParticleSystem _particles;

    public float GetMovingSpeed()
    {
        return _movingSpeed;
    }

    public override void ResetData()
    {
        base.ResetData();
        _particles.Play();
    }

    private void Start()
    {
        StartCoroutine(ReleaseByTime());
    }

    private IEnumerator ReleaseByTime()
    {
        yield return new WaitForSeconds(_lifetime);
        TargetReachedEvent.Invoke();
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * _movingSpeed, Space.World);
    }
}
