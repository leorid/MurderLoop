using UnityEngine;

namespace JL
{
	/// <summary>
	/// Source: https://forum.unity.com/threads/use-torque-to-match-rotation-to-a-second-object.446422/
	/// </summary>
	public class SpringRotation : MonoBehaviour
	{
		public bool active = true;

		public Transform target;
		[SerializeField] float _force = 2;
		[SerializeField] float _maxForce = 2;
		[SerializeField] float _forceReduction = 1;
		private new Rigidbody _rb;

		private Vector3 torque;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
		}

		//private void OnGUI()
		//{
		//	Quaternion diff = Quaternion.Inverse(_rb.rotation) * target.rotation;
		//	Vector3 eulers = OrientTorque(diff.eulerAngles);
		//	GUI.TextArea(new Rect(50, 50, 200, 50), eulers.ToString());
		//}

		private void FixedUpdate()
		{
			if (active)
			{
				//Find the rotation difference in eulers
				Quaternion diff = Quaternion.Inverse(_rb.rotation) * target.rotation;
				Vector3 eulers = OrientTorque(diff.eulerAngles);
				Vector3 torque = eulers;
				//put the torque back in body space
				torque = _rb.rotation * torque;

				// substract current angular velocity to prevent overshoot
				torque -= _rb.angularVelocity * _forceReduction;

				torque *= _force;
				if (torque.magnitude > _maxForce) torque = torque.normalized * _maxForce;

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