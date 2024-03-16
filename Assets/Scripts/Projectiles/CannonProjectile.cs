using UnityEngine;

public class CannonProjectile : Projectile
{
    private void FixedUpdate()
    {
        transform.Translate(transform.forward * _movingSpeed);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Monster>(out var monster))
        {
            monster.ApplyDamage(_damage);
            TargetReachedEvent.Invoke();
        }
    }
}
