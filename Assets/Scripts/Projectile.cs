using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public UnityEvent TargetReachedEvent = new UnityEvent();
    
    [SerializeField] protected float _movingSpeed = 0.2f;
    [SerializeField] protected int _damage = 10;

    protected Transform _target;

    public float m_speed = 0.2f;
    public int m_damage = 10;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void Reset()
    {

    }
}
