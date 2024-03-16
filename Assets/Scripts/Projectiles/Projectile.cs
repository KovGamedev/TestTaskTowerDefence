using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public UnityEvent TargetReachedEvent = new UnityEvent();

    [SerializeField] protected float _movingSpeed = 0.2f;
    [SerializeField] protected int _damage = 10;
}
