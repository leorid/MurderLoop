using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class SnapToTarget : MonoBehaviour
	{
		[SerializeField] Transform _target;
		[SerializeField] Transform _floorTarget;
		[SerializeField] float _maxDistance = 1;
		[SerializeField] float _minDistance = 0.1f;
		[SerializeField] SnapToTarget _other;
		TargetJoint _targetJoint;
		[SerializeField] bool _snapping = true;
		[SerializeField] float _distance;
		LayerMask _mask;

		Rigidbody _rb;

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
			_targetJoint = GetComponent<TargetJoint>();
			_mask = GetComponentInParent<CharacterMove>().mask;
		}

		private void Update()
		{
			if (_snapping) return;
			_distance = (transform.position - _target.position).magnitude;
			if (_distance > _maxDistance && 
				(!_other._targetJoint.enabled || !_other._snapping))
			{
				_snapping = true;
				_targetJoint.target = _target;
			}
		}

		private void FixedUpdate()
		{
			if (!_snapping || !_target) return;

			_distance = (transform.position - _target.position).magnitude;
			if (_distance < _minDistance)
			{
				Vector3 newPos;
				if (Physics.Raycast(transform.position, Vector3.down,
					out RaycastHit hit, 0.3f, _mask))
				{
					newPos = hit.point;
				}
				else
				{
					newPos = transform.position;
				}

				_snapping = false;
				_floorTarget.position = newPos;
				_targetJoint.target = _floorTarget;
			}
		}
	}
}
