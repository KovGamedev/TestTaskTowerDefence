using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterSpawner : Spawner
{
    [SerializeField] private Transform _monsterMovingTarget;
    [SerializeField] protected float _spawnDelay = 2f;
    [SerializeField] protected float _spawnInterval = 3f;

    private List<Monster> _activeMonsters = new List<Monster>();

    public List<Monster> GetActiveMonsters()
    {
        return new List<Monster>(_activeMonsters);
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
        return gameObject;
    }

    private void ReleaseMonster(GameObject item)
    {
        var monster = item.GetComponent<Monster>();
        monster.DeathEvent.RemoveAllListeners();
        monster.TargetReachedEvent.RemoveAllListeners();
        _activeMonsters.Remove(monster);
        item.SetActive(false);
    }

    private void AcquireMonster(GameObject item)
    {
        var directionToTarget = _monsterMovingTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        item.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f));
        var monster = item.GetComponent<Monster>();
        monster.Reset();
        monster.DeathEvent.AddListener(() => _pool.Release(item));
        monster.TargetReachedEvent.AddListener(() => _pool.Release(item));
        _activeMonsters.Add(monster);
        item.SetActive(true);
    }
}
