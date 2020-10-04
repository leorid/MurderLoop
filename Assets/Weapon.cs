using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] Rigidbody _projectilePrefab;
		[SerializeField] float _fireRate = 0.2f;
		float _rnd = 0.05f;
		float _lastFireTime = 0;
		[SerializeField] bool _aiControlled;
		[SerializeField] TargetJoint _requiredJoint;
		Collider[] cols;

		Queue<Rigidbody> _pool = new Queue<Rigidbody>();
		Transform _camTransform;

		void Start()
		{
			cols = transform.parent.GetComponentsInChildren<Collider>();
			_camTransform = Camera.main.transform;
		}

		void Update()
		{
			if (_aiControlled) return;

			// get wanted projectile rotation
			Vector3 targetPos = _camTransform.position + _camTransform.forward * 50;
			if (Physics.Raycast(_camTransform.position, _camTransform.forward,
				out RaycastHit hit, 50))
			{
				targetPos = hit.point;
			}

			if (Input.GetMouseButton(0)) Fire(targetPos);
		}

		public bool Alive()
		{
			return !_requiredJoint || _requiredJoint.enabled;
		}

		public void Fire(Vector3 targetPos)
		{
			if (!Alive()) return;

			if (_lastFireTime + _fireRate < Time.time)
			{
				_lastFireTime = Time.time;

				Vector3 dir = targetPos - transform.position;
				dir.Normalize();
				dir += Random.insideUnitSphere * _rnd;

				Quaternion wantedRot = Quaternion.LookRotation(dir);


				Rigidbody projectile;
				if (_pool.Count > 0)
				{
					projectile = _pool.Dequeue();
					projectile.transform.position = transform.position;
					projectile.transform.rotation = wantedRot;
					projectile.gameObject.SetActive(true);
					projectile.GetComponent<TrailRenderer>().Clear();
				}
				else
				{
					projectile = Instantiate(_projectilePrefab, transform.position, wantedRot);
					var projectileClass = projectile.GetComponent<Projectile>();
					projectileClass.weapon = this;
					projectileClass.isEnemyProjectile = _aiControlled;
				}
			}
		}

		public void AddToPool(Rigidbody projectile)
		{
			_pool.Enqueue(projectile);
		}
	}
}