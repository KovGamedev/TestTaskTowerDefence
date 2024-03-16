using UnityEngine;

public class ProjectilesSpawner : Spawner
{
    public Projectile GetProjectile()
    {
        return _pool.Acquire().GetComponent<Projectile>();
    }

    protected override Pool<GameObject> CreatePool()
    {
        return new Pool<GameObject>(CreateProjectile, AcquireProjectile, ReleaseProjectile);
    }

    private GameObject CreateProjectile()
    {
        var gameObject = Instantiate(_spawnablePrefab, transform.position, Quaternion.identity);
        gameObject.GetComponent<Projectile>().TargetReachedEvent.AddListener(() => _pool.Release(gameObject));
        return gameObject;
    }

    private void ReleaseProjectile(GameObject item)
    {
        item.SetActive(false);
    }

    private void AcquireProjectile(GameObject item)
    {
        item.transform.position = transform.position;
        item.GetComponent<Projectile>().Reset();
        item.SetActive(true);
    }
}
