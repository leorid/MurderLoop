using JL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class AI : MonoBehaviour
	{
		[SerializeField] float _shotDistance = 15;
		[SerializeField] float _triggerStartDistance = 40;
		[SerializeField] float _triggerEndDistance = 50;
		[SerializeField] float _rndFactor = 2;
		[SerializeField] float _knockbackForce = 20;
		public static Transform target;
		CharacterMove _mover;
		[SerializeField] bool _isTriggered;
		[SerializeField] Weapon weapon1;
		[SerializeField] Weapon weapon2;
		[SerializeField] BodyPart head;
		[SerializeField] Rigidbody _rb;

		public bool IsDead
		{
			get
			{
				return !head || head.health == 0;
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			_mover = GetComponent<CharacterMove>();
			_rb = GetComponent<Rigidbody>();
			head.OnDieEvent -= Head_OnDieEvent;
			head.OnDieEvent += Head_OnDieEvent;
		}

		private void Head_OnDieEvent()
		{
			enabled = false;
		}

		public void GotDamage(int damage)
		{
			if(damage < 5 && target)
			{
				// knockback
				_rb.AddForce((transform.position - target.position).normalized *
					_knockbackForce, ForceMode.Impulse);
			}
		}

		// Update is called once per frame
		void Update()
		{
			if(Mathf.Abs(transform.position.y) > 50)
			{
				if (head) head.Damage(10000);
			}

			if (!target) return;

			// move to player until shoot distance
			Vector3 targetDir = (target.transform.position - transform.position);
			float targetDist = targetDir.magnitude;

			_mover.SetMove(Vector3.zero);

			if (_isTriggered)
			{
				_mover.SetRotate(targetDir);

				if (targetDist > _triggerEndDistance)
				{
					_isTriggered = false;
					return;
				}

				if (targetDist > _shotDistance ||
					(!weapon1.Alive() && !weapon2.Alive()))
				{
					_mover.SetMove(targetDir.normalized);
				}
				else
				{
					// shoot
					Vector3 rnd1 = Random.insideUnitSphere * _rndFactor;
					weapon1.Fire(target.position + rnd1);
					Vector3 rnd2 = Random.insideUnitSphere * _rndFactor;
					weapon2.Fire(target.position + rnd2);
				}
			}
			else
			{
				//_mover.SetRotate(targetDir);

				if (targetDist < _triggerStartDistance)
				{
					_isTriggered = true;
				}
			}
		}
	}
}
