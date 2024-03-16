using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProjectilesSpawner))]
public abstract class Tower : MonoBehaviour
{
    [SerializeField] private MonsterSpawner _monsterSpawner;
    [SerializeField] protected float _shootingInterval = 0.5f;
    [SerializeField] protected float _shootingRange = 4f;

    protected ProjectilesSpawner _projectilesSpawner;
    protected Monster _nearestMonster;

    protected void Awake()
    {
        _projectilesSpawner = GetComponent<ProjectilesSpawner>();
    }

    protected void Start()
    {
        StartCoroutine(FireIfPossible());
    }

    // TODO Разделить на две корутины? Одна ищет врагов, а другая стреляет?
    protected IEnumerator FireIfPossible()
    {
        yield return new WaitUntil(() => {
            _nearestMonster = GetNearestMonster();
            return _nearestMonster != null;
        });

        _nearestMonster.TargetReachedEvent.AddListener(StopShootingIfMonsterDestroyed);
        _nearestMonster.DeathEvent.AddListener(StopShootingIfMonsterDestroyed);

        yield return new WaitUntil(() => Vector3.Distance(transform.position, _nearestMonster.transform.position) <= _shootingRange);

        CreateProjectile();

        yield return new WaitForSeconds(_shootingInterval);

        StartCoroutine(FireIfPossible());
    }

    protected Monster GetNearestMonster()
    {
        return _monsterSpawner.GetActiveMonsters()
            .OrderBy(monster => (monster.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
    }

    protected void StopShootingIfMonsterDestroyed()
    {
        StopAllCoroutines();
        StartCoroutine(FireIfPossible());
    }

    protected abstract void CreateProjectile();
}
