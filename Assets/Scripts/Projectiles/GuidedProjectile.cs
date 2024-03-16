using UnityEngine;

public class GuidedProjectile : Projectile
{
    protected Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _movingSpeed);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Monster>(out var monster))
        {
            monster.ApplyDamage(_damage);
            TargetReachedEvent.Invoke();
        }
    }
}
