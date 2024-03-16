using UnityEngine;

public class GuidedProjectile : Projectile
{
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _movingSpeed);
    }
}
