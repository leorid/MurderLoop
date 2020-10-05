using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JL
{
	public class Player : MonoBehaviour
	{
		[SerializeField] float _factor = 2;
		public Transform camFollowTarget;
		[SerializeField] PrefabReferenceHelper _playerPrefab;
		[SerializeField] BodyPart _head;
		[SerializeField] AudioSource _hurtSound;

		float _dieTimer;

		void Start()
		{
			AI.target = transform;

			BodyPart[] parts = GetComponentsInChildren<BodyPart>();
			foreach (BodyPart part in parts)
			{
				part.health = Mathf.RoundToInt(part.health * _factor);
			}

			_dieTimer = 0;
		}

		private void Update()
		{
			if(!_head || _head.health <= 0)
			{
				_dieTimer += Time.deltaTime;
			}
			if (_dieTimer > 2 || Mathf.Abs(transform.position.y) > 50)
			{
				Respawn(Checkpoint.respawnPosition);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				Respawn(Checkpoint.respawnPosition);
			}
		}

		public void Respawn(Transform spawnPoint)
		{
			if(spawnPoint.TryGetComponent(out Checkpoint cp))
			{
				cp.OnRespawn();
			}

			EnemySpawner.InvokeClearEvent();

			GameObject newPlayerGO =
				Instantiate(_playerPrefab.reference, spawnPoint.position,
				spawnPoint.rotation, transform.parent.parent);

			Player newPlayer = newPlayerGO.GetComponentInChildren<Player>();

			void AlignAndDestroy(Transform oldT, Transform newT)
			{
				oldT.parent = newT.parent;
				oldT.position = newT.position;
				oldT.rotation = newT.rotation;

				Destroy(newT.gameObject);
			}

			AlignAndDestroy(camFollowTarget, newPlayer.camFollowTarget);
			AlignAndDestroy(newPlayer.camFollowTarget.GetChild(0),
				camFollowTarget.GetChild(0));

			newPlayer.camFollowTarget = camFollowTarget;

			Destroy(transform.parent.gameObject);
		}

		public void GotDamage(int damage)
		{
			if (_hurtSound) _hurtSound.Play();
			FlashDamage.Flash();
		}
	}
}
