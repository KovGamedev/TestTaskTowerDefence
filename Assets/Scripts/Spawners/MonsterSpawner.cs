using Boo.Lang;
using System.Collections;
using UnityEngine;

public class MonsterSpawner : Spawner
{
    [SerializeField] private Transform _monsterMovingTarget;
    [SerializeField] protected float _spawnDelay = 2f;
    [SerializeField] protected float _spawnInterval = 3f;

    private List<Transform> _activeMonsters = new List<Transform>();

    public List<Transform> GetActiveMonsters()
    {
        return new List<Transform>(_activeMonsters);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Spawn(_spawnDelay));
    }

    protected IEnumerator Spawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.Acquire();
        StartCoroutine(Spawn(_spawnInterval));
    }

    protected override Pool<GameObject> CreatePool()
    {
        return new Pool<GameObject>(CreateMonster, AcquireMonster, ReleaseMonster);
    }

    private GameObject CreateMonster()
    {
        var gameObject = Instantiate(_spawnablePrefab, transform.position, Quaternion.identity);
        var monster = gameObject.GetComponent<Monster>();
        monster.SetTarget(_monsterMovingTarget);
        monster.TargetReachedEvent.AddListener(() => _pool.Release(gameObject));
        monster.DeathEvent.AddListener(() => _pool.Release(gameObject));
        return gameObject;
    }

    private void ReleaseMonster(GameObject item)
    {
        item.SetActive(false);
        _activeMonsters.Remove(item.transform);
    }

    private void AcquireMonster(GameObject item)
    {
        item.transform.position = transform.position;
        item.GetComponent<Monster>().Reset();
        item.SetActive(true);
        _activeMonsters.Add(item.transform);
    }
}
