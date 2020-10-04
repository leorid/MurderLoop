using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] float _speed = 10;
		[SerializeField] float _force = 1;
		[System.NonSerialized] public bool isEnemyProjectile = true;
		public Weapon weapon;
		Rigidbody _rb;
		AudioSource _source;

		Vector3 lastPosition;
		Vector3 startPosition;

		void Start()
		{
			_rb = GetComponent<Rigidbody>();
			_source = GetComponent<AudioSource>();
		}

		void FixedUpdate()
		{
			if (!enabled) return;

			if((startPosition - transform.position).magnitude > 30)
			{
				gameObject.SetActive(false);
				weapon.AddToPool(_rb);
				return;
			}

			_rb.velocity = transform.forward * _speed;

			Vector3 dir = transform.position - lastPosition;
			if (Physics.Raycast(lastPosition, dir, out RaycastHit hit,
				dir.magnitude + 0.1f))
			{
				if(isEnemyProjectile && hit.collider.CompareTag("Enemy"))
				{
					return;
				}
				if (!isEnemyProjectile && hit.collider.CompareTag("Player"))
				{
					return;
				}

				if (hit.collider.attachedRigidbody)
				{
					hit.collider.attachedRigidbody.AddForce(transform.forward * _force,
						ForceMode.Impulse);
				}
				hit.collider.SendMessage("Damage", 1,
				SendMessageOptions.DontRequireReceiver);
				gameObject.SetActive(false);
				weapon.AddToPool(_rb);
			}
		}

		private void OnEnable()
		{
			lastPosition = transform.position;
			startPosition = transform.position;

			if(_source) _source.pitch = Random.Range(0.8f, 1.2f);
		}
	}
}
