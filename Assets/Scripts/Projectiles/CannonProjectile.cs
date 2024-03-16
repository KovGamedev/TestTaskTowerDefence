using System.Collections;
using UnityEngine;

public class CannonProjectile : Projectile
{
    [SerializeField, Min(0f)] private float _lifetime;

    private void Start()
    {
        StartCoroutine(Release());
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(_lifetime);
        TargetReachedEvent.Invoke();
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * _movingSpeed, Space.World);
    }
}
