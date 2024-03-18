using UnityEngine;

public class ProjectilesSpawner : Spawner
{
    [SerializeField] private Transform _spawnPoint;

    private GameObject _currentProjectile;

    public void DirectAndActivateProjectile()
    {
        _currentProjectile.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
        _currentProjectile.GetComponent<Projectile>().ResetData();
        _currentProjectile.SetActive(true);
    }

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
        var gameObject = Instantiate(_spawnablePrefab, _spawnPoint.position, _spawnPoint.rotation);
        gameObject.GetComponent<Projectile>().TargetReachedEvent.AddListener(() => _pool.Release(gameObject));
        return gameObject;
    }

    private void ReleaseProjectile(GameObject item)
    {
        item.SetActive(false);
    }

    private void AcquireProjectile(GameObject item)
    {
        _currentProjectile = item;
    }
}
