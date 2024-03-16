public class CannonProjectile : Projectile
{
    private void FixedUpdate()
    {
        transform.Translate(transform.forward * _movingSpeed);
    }
}
