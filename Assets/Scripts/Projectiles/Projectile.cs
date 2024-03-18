using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public UnityEvent TargetReachedEvent = new UnityEvent();

    [SerializeField] protected float _movingSpeed = 0.2f;
    [SerializeField] protected int _damage = 10;

    public virtual void ResetData()
    { }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Monster>(out var monster))
        {
            monster.ApplyDamage(_damage);
            TargetReachedEvent.Invoke();
        }
    }
}
