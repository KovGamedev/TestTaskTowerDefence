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
        StartCoroutine(FindNearestMonster());
        StartCoroutine(FireIfPossible());
    }

    protected IEnumerator FindNearestMonster()
    {
        yield return new WaitUntil(() => {
            _nearestMonster = GetNearestMonster();
            return _nearestMonster != null;
        });
        yield return new WaitUntil(() => {
            var isMonsterOutOfRange = Vector3.Distance(transform.position, _nearestMonster.transform.position) > _shootingRange;
            var isMonsterActive = _nearestMonster.gameObject.activeSelf;
            return isMonsterOutOfRange || isMonsterActive;
        });
        StartCoroutine(FindNearestMonster());
    }

    protected Monster GetNearestMonster()
    {
        return _monsterSpawner.GetActiveMonsters()
            .FindAll(monster => Vector3.Distance(transform.position, monster.transform.position) <= _shootingRange)
            .OrderBy(monster => (monster.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
    }

    protected IEnumerator FireIfPossible()
    {
        yield return new WaitUntil(() => _nearestMonster != null);
        CreateProjectile();
        yield return new WaitForSeconds(_shootingInterval);
        StartCoroutine(FireIfPossible());
    }

    protected abstract void CreateProjectile();
}
