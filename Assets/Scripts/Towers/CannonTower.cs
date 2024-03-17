using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonTower : Tower
{
    [SerializeField] private Transform _cannonBase;
    [SerializeField] private Transform _cannonBarrel;
    [SerializeField] private Transform _cannonballSpawnPoint;

    private float _projectileMovementSpeed;

    protected override void CreateProjectile()
    {
        _projectileMovementSpeed = (_projectilesSpawner.GetProjectile() as CannonProjectile).GetMovingSpeed();
        StartCoroutine(DirectAndActivateProjectile());
    }

    private IEnumerator DirectAndActivateProjectile()
    {
        yield return new WaitUntil(() => _isTowerDirected);
        _projectilesSpawner.DirectAndActivateProjectile();
    }

    private void FixedUpdate()
    {
        if (_nearestMonster == null || !_nearestMonster.gameObject.activeSelf)
        {
            _isTowerDirected = false;
            return;
        }
        Debug.DrawLine(_cannonballSpawnPoint.position, GetTargetWithOffset());

        var directionToHitPoint = GetTargetWithOffset() - _cannonBarrel.position;
        Quaternion rotation = Quaternion.LookRotation(directionToHitPoint, Vector3.up);
        _cannonBase.transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        _cannonBarrel.transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0f, 0f);
        _isTowerDirected = true;
    }

    private Vector3 GetTargetWithOffset() {
        // Xmeeting = Xm0 + Vm * t      <- Monster
        // Xmeeting = Xp0 + Vp * t      <- Projectile
        // Xm0 + Vm * t = Xp0 + Vp * t  <- Because Xmeeting is the same
        // Xm0 + Vm * t - Xp0 = Vp * t
        // Xm0 - Xp0 = Vp * t - Vm * t
        // Xm0 - Xp0 = t * (Vp - Vm)
        // t = (Xm0 - Xp0) / (Vp - Vm)

        var futureSpawnPoint = new Ray(_cannonBarrel.position, _nearestMonster.transform.position).GetPoint(_cannonballSpawnPoint.localPosition.z);
        var timeToHit = Vector3.Distance(_nearestMonster.transform.position, futureSpawnPoint) /
            (_projectileMovementSpeed - _nearestMonster.GetMovingSpeed());
        return _nearestMonster.transform.position + _nearestMonster.transform.forward * _nearestMonster.GetMovingSpeed() * timeToHit;
    }
}

