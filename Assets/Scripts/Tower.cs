using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ProjectilesSpawner))]
public class Tower : MonoBehaviour
{
    [SerializeField] private MonsterSpawner _monsterSpawner;
    [SerializeField] protected float _shootingInterval = 0.5f;
    [SerializeField] protected float _shootingRange = 4f;

    protected ProjectilesSpawner _projectilesSpawner;

    protected void Awake()
    {
        _projectilesSpawner = GetComponent<ProjectilesSpawner>();
    }

    protected Transform GetNearestMonster()
    {
        return _monsterSpawner.GetActiveMonsters()
            .OrderBy(monsterTransform => (monsterTransform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
    }
}
