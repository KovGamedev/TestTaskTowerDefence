using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    [HideInInspector] public UnityEvent TargetReachedEvent = new UnityEvent();

    [SerializeField] private float _movingSpeed = 0.1f;
    [SerializeField] private int _maxHealth = 30;

    private int _currentHealth;
    private Transform _movementTarget;

    public void SetTarget(Transform movementTarget)
    {
        _movementTarget = movementTarget;
    }

    public void Reset()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        Reset();
    }

    private void FixedUpdate()
    {
        if (_movementTarget == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _movementTarget.position, _movingSpeed);

        if (transform.position == _movementTarget.position)
            TargetReachedEvent.Invoke();
    }

    private void OnDestroy()
    {
        TargetReachedEvent.RemoveAllListeners();
    }
}
