using UnityEngine;

public class GuidedProjectile : Projectile
{
    protected Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update () {
        if (_target == null) {
			Destroy (gameObject);
			return;
		}

		var translation = _target.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		//TODO Закомментировал, чтобы скомпилилось
		//monster._currentHealth -= m_damage;
		//if (monster._currentHealth <= 0) {
		//	Destroy (monster.gameObject);
		//}
		Destroy (gameObject);
	}
}
