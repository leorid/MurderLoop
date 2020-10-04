using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class TargetJoint : MonoBehaviour
	{
		public Transform target;
		public bool positionActive = true;
		[SerializeField] float moveForce = 1;
		[SerializeField] float maxMoveForce = 1;
		[SerializeField] float _moveForceReduction = 1;
		public bool rotationActive = true;
		[SerializeField] float rotateForce = 0.5f;
		[SerializeField] float maxRotateForce = 20;
		[SerializeField] float _rotateForceReduction = 1;
		[SerializeField] float _limitAngle = -1;
		Rigidbody _rb;

		Quaternion _initRotation;

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
			_initRotation = transform.localRotation;
		}

		void FixedUpdate()
		{
			if (!target) return;

			if (positionActive)
			{
				Vector3 move = target.position - transform.position;
				move *= moveForce;
				Vector3 moveDiff = move - _rb.velocity * _moveForceReduction;
				if (moveDiff.magnitude > maxMoveForce)
					moveDiff = moveDiff.normalized * maxMoveForce;
				_rb.AddForce(moveDiff, ForceMode.VelocityChange);
			}

			if (rotationActive)
			{
				Quaternion targetRot;
				if(_limitAngle > 1 && 
					Vector3.Angle(-transform.parent.forward, target.up) > _limitAngle)
				{
					targetRot = transform.parent.rotation * _initRotation;
				}
				else
				{
					targetRot = target.rotation;
				}

				//Find the rotation difference in eulers
				Quaternion diff = Quaternion.Inverse(_rb.rotation) * targetRot;
				Vector3 eulers = OrientTorque(diff.eulerAngles);
				Vector3 torque = eulers;
				//put the torque back in body space
				torque = _rb.rotation * torque;

				// substract current angular velocity to prevent overshoot
				torque -= _rb.angularVelocity * _rotateForceReduction;

				torque *= rotateForce;
				if (torque.magnitude > maxRotateForce) 
					torque = torque.normalized * maxRotateForce;

				_rb.AddTorque(torque, ForceMode.VelocityChange);
			}
		}

		private Vector3 OrientTorque(Vector3 torque)
		{
			// Quaternion's Euler conversion results in (0-360)
			// For torque, we need -180 to 180.

			if (torque.x > 180) torque.x -= 360;
			if (torque.y > 180) torque.y -= 360;
			if (torque.z > 180) torque.z -= 360;

			return torque;
		}
	}
}
