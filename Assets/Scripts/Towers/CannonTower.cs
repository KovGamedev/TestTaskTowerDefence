using UnityEngine;

public class CannonTower : Tower
{
    [SerializeField] private Transform _cannonBase;
    [SerializeField] private Transform _cannonBarrel;

    protected override void CreateProjectile()
    {
        _projectilesSpawner.GetProjectile();
    }

    private void FixedUpdate()
    {
        if (_nearestMonster == null)
            return;

        var directionToEnemy = _nearestMonster.position - _cannonBarrel.position;
        Quaternion rotation = Quaternion.LookRotation(directionToEnemy, Vector3.up);
        _cannonBase.transform.localRotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        _cannonBarrel.transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0f, 0f);
    }
}

