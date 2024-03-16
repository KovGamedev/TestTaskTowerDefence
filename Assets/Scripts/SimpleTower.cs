using System.Collections;
using UnityEngine;

public class SimpleTower : Tower
{
	private Transform _nearestMonster;

	private void Start()
	{
		StartCoroutine(FireIfPossible());
    }

	private IEnumerator FireIfPossible()
	{
        yield return new WaitUntil(() => {
			_nearestMonster = GetNearestMonster();
			return _nearestMonster != null;
        });
        
		yield return new WaitUntil(() => Vector3.Distance(transform.position, _nearestMonster.position) <= _shootingRange);

        _projectilesSpawner.GetProjectile().SetTarget(_nearestMonster);

        yield return new WaitForSeconds(_shootingInterval);

        StartCoroutine(FireIfPossible());
    }
}
