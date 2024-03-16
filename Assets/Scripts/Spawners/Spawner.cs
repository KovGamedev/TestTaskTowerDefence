using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject _spawnablePrefab;

    protected Pool<GameObject> _pool;

    protected virtual void Start()
    {
        _pool = CreatePool();
    }

    protected abstract Pool<GameObject> CreatePool();
}
