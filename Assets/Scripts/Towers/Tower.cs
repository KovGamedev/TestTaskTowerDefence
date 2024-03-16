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
    protected Transform _nearestMonster;

    protected void Awake()
    {
        _projectilesSpawner = GetComponent<ProjectilesSpawner>();
    }

    protected void Start()
    {
        StartCoroutine(FireIfPossible());
    }

    protected IEnumerator FireIfPossible()
    {
        yield return new WaitUntil(() => {
            _nearestMonster = GetNearestMonster();
            return _nearestMonster != null;
        });

        yield return new WaitUntil(() => Vector3.Distance(transform.position, _nearestMonster.position) <= _shootingRange);

        CreateProjectile();

        yield return new WaitForSeconds(_shootingInterval);

        StartCoroutine(FireIfPossible());
    }

    protected Transform GetNearestMonster()
    {
        return _monsterSpawner.GetActiveMonsters()
            .OrderBy(monsterTransform => (monsterTransform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
    }

    protected abstract void CreateProjectile();
}
