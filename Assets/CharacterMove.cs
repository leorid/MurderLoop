using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class CharacterMove : MonoBehaviour
	{
		[SerializeField] float _height = 1.1775f;
		[SerializeField] float _moveSpeed = 1;
		[SerializeField] float _rotateSpeed = 1;
		[SerializeField] float _jumpForce = 1;
		[SerializeField] float _stableForceGrav = 1;
		[SerializeField] float _stableForce = 1;
		public LayerMask mask = ~0;
		[SerializeField] float _footDistance = 0.5f;
		[SerializeField] Transform _footTargets;
		[SerializeField] TargetJoint _footJointL;
		[SerializeField] TargetJoint _footJointR;
		[SerializeField] bool _aiDriven;

		[SerializeField] BodyPart[] legs;
		[SerializeField] BodyPart[] arms;
		[SerializeField] BodyPart head;
		[SerializeField] Material _valuableHeadMat;
		[SerializeField] AudioSource _jumpSound;

		float _jumpDelay = 0.5f;
		float _lastJumpTime = 0;

		Vector3 _moveDirection;
		Vector3 _lookDirection;
		bool _jumpInput;

		Camera _cam;
		Rigidbody _rb;
		Quaternion _initRot;

		// Start is called before the first frame update
		void Start()
		{
			_cam = Camera.main;
			_rb = GetComponent<Rigidbody>();
			_initRot = transform.rotation;

			Collider[] cols = GetComponentsInChildren<Collider>();
			foreach (Collider colA in cols)
			{
				foreach (Collider colB in cols)
				{
					Physics.IgnoreCollision(colA, colB);
				}
			}
			head.OnDieEvent += Head_OnDieEvent;
		}

		private void Head_OnDieEvent()
		{
			head.gameObject.tag = "Head";
			head.GetComponent<MeshRenderer>().sharedMaterial = _valuableHeadMat;
			Destroy(this);
			if (!_aiDriven)
			{
				// Game Over
			}

			enabled = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (_aiDriven) return;

			Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			Vector3 camForward = _cam.transform.forward;
			camForward.y = 0;
			camForward.Normalize();
			_moveDirection = _cam.transform.right * moveInput.x + camForward * moveInput.y;
			if (_moveDirection.magnitude > 1) _moveDirection.Normalize();


			if (Input.GetKeyDown(KeyCode.Space)) { _jumpInput = true; }
		}

		public void SetMove(Vector3 move)
		{
			move.y = 0;
			_moveDirection = move;
		}
		public void SetRotate(Vector3 look)
		{
			look.y = 0;
			_lookDirection = look;
		}

		void FixedUpdate()
		{
			if (!head) return;

			// move
			float moveMulti = 1;
			if (legs[0].health <= 0) moveMulti *= 0.5f;
			if (legs[1].health <= 0) moveMulti *= 0.5f;
			_moveDirection *= _moveSpeed * moveMulti;

			Vector3 velocityNoY = _rb.velocity;
			velocityNoY.y = 0;
			Vector3 diff = _moveDirection - velocityNoY;

			_rb.AddForce(diff, ForceMode.VelocityChange);
			//_rb.velocity = new Vector3(_moveDirection.x, _rb.velocity.y, _moveDirection.z);

			// rotate towards
			if (_aiDriven && _lookDirection.magnitude > 0.01f)
			{
				_rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation,
					Quaternion.LookRotation(_lookDirection, Vector3.up), _rotateSpeed));
			}
			else if (_moveDirection.sqrMagnitude > 0.1f)
			{
				Vector3 camForward = _cam.transform.forward;
				camForward.y = 0;
				camForward.Normalize();

				_rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation,
					Quaternion.LookRotation(camForward, Vector3.up), _rotateSpeed));
			}

			if (_lastJumpTime + _jumpDelay > Time.time)
			{
				_footJointL.enabled = false;
				_footJointR.enabled = false;
				return;
			}

			// keep height
			RaycastHit hit;

			float tempHeight = _height;

			if (legs[0].health <= 0 && legs[1].health <= 0)
			{
				tempHeight = 0.5f;
			}

			if (!Physics.Raycast(transform.position, Vector3.down, out hit,
				tempHeight + 0.5f, mask))
			{
				_jumpInput = false;

				_footJointL.enabled = false;
				_footJointR.enabled = false;

				return;
			}
			if (legs[0].health > 0) _footJointL.enabled = true;
			if (legs[1].health > 0) _footJointR.enabled = true;

			Vector3 footDir = _moveDirection;
			if (footDir.magnitude > _footDistance)
				footDir = footDir.normalized * _footDistance;
			_footTargets.transform.position = hit.point + footDir;


			// grounded
			float difference = tempHeight - (hit.point - _rb.position).magnitude;

			_rb.velocity = new Vector3(
				_rb.velocity.x,
				difference * _stableForce - _rb.velocity.y,
				_rb.velocity.z);

			//_rb.AddForce(Vector3.up * difference * difference * _stableForce -
			//	Physics.gravity * _stableForceGrav, ForceMode.VelocityChange);

			//jump
			if (_jumpInput)
			{
				if(_jumpSound) _jumpSound.Play();

				_footJointL.enabled = false;
				_footJointR.enabled = false;
				_jumpInput = false;
				_lastJumpTime = Time.time;
				_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
			}
		}
	}
}
