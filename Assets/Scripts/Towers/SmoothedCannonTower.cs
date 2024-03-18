using System.Collections;
using UnityEngine;

public class SmoothedCannonTower : Tower
{
    [SerializeField] private Transform _cannonBase;
    [SerializeField] private Transform _cannonBarrel;
    [SerializeField] private Transform _cannonballSpawnPoint;
    [SerializeField] private ParticleSystem _particles;
    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float _yawSpeed;
    [SerializeField, Range(0f, 1f)] private float _pitchSpeed;

    private float _projectileMovementSpeed;
    private float _yawPercent;
    private float _pitchPercent;

    protected override void Start()
    {
        base.Start();
        _isTowerDirected = false;
    }

    protected override void CreateProjectile()
    {
        _projectileMovementSpeed = (_projectilesSpawner.GetProjectile() as CannonProjectile).GetMovingSpeed();
        StartCoroutine(DirectAndActivateProjectile());
    }

    protected override void HandleLosing()
    {
        _isTowerDirected = false;
        _yawPercent = 0f;
        _pitchPercent = 0f;
    }

    private IEnumerator DirectAndActivateProjectile()
    {
        yield return new WaitUntil(() => _isTowerDirected);
        _projectilesSpawner.DirectAndActivateProjectile();
        _particles.Play();
    }

    private void FixedUpdate()
    {
        if (_nearestMonster == null || !_nearestMonster.gameObject.activeSelf)
            return;

        var directionToHitPoint = GetTargetWithOffset() - _cannonBarrel.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToHitPoint, Vector3.up);
        ChangeYaw(targetRotation);
        ChangePitch(targetRotation);
        _isTowerDirected = 1f <= _yawPercent && 1f <= _pitchPercent;
    }

    private void ChangeYaw(Quaternion targetRotation)
    {
        var cannonBaseTargetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        _cannonBase.rotation = Quaternion.Lerp(_cannonBase.rotation, cannonBaseTargetRotation, _yawPercent);
        _yawPercent += _yawSpeed;
    }

    private void ChangePitch(Quaternion targetRotation)
    {
        var cannonBarrelTargetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, 0f, 0f);
        _cannonBarrel.localRotation = Quaternion.Lerp(_cannonBarrel.localRotation, cannonBarrelTargetRotation, _pitchPercent);
        _pitchPercent += _pitchSpeed;
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
        var monsterSpeedSign = Vector3.Angle(
            _nearestMonster.transform.forward,
            _cannonBarrel.position - _nearestMonster.transform.position
        ) < 90 ? 1 : -1;
        var timeToHit = Vector3.Distance(_nearestMonster.transform.position, futureSpawnPoint) /
            (_projectileMovementSpeed + monsterSpeedSign * _nearestMonster.GetMovingSpeed());
        return _nearestMonster.transform.position + _nearestMonster.transform.forward * _nearestMonster.GetMovingSpeed() * timeToHit;
    }
}

