using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class BodyPart : MonoBehaviour
	{
		Joint _joint;
		TargetJoint[] _myJoints;
		BodyPart[] _subParts;
		[SerializeField] GameObject[] _bloodCubes;

		public int health = 5;
		[System.NonSerialized] int _myHealth;

		public delegate void OnDie();
		public event OnDie OnDieEvent;

		Transform topParent;

		public void InitBodyPart(CharacterTop characterTop)
		{
			topParent = characterTop.transform;
		}

		private void Start()
		{
			_myHealth = health;
			_joint = GetComponent<Joint>();
			_myJoints = GetComponentsInChildren<TargetJoint>();
			_subParts = GetComponentsInChildren<BodyPart>();
			foreach (GameObject bloodCube in _bloodCubes)
			{
				bloodCube.SetActive(false);
			}
		}

		public void Damage(int damage)
		{
			_myHealth -= damage;
			if (_myHealth < health) health = _myHealth;

			SendMessageUpwards("GotDamage", damage, SendMessageOptions.DontRequireReceiver);

			if (_myHealth <= 0)
			{
				OnDieEvent?.Invoke();
				OnDieEvent = null;

				foreach (BodyPart subPart in _subParts)
				{
					subPart.health = 0;
				}
				foreach (GameObject bloodCube in _bloodCubes)
				{
					bloodCube.SetActive(true);
				}

				if (_joint) Destroy(_joint);

				if (TryGetComponent(out HeadHealth headHealth))
				{
					transform.SetParent(null);
				}
				else
				{
					transform.SetParent(topParent);
				}

				foreach (TargetJoint _myJoint in _myJoints)
					_myJoint.enabled = false;
			}
		}
	}
}
