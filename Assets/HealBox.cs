using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JL
{
	public class HealBox : MonoBehaviour
	{
		[SerializeField] GameObject _playerPrefab;
		[SerializeField] Transform _spawnPosition;
		[SerializeField] UnityEvent _onHeal;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player oldPlayer))
			{
				Transform oldPlayerParentT = oldPlayer.transform.parent;

				GameObject newPlayerGO = 
					Instantiate(_playerPrefab, _spawnPosition.position,
					_spawnPosition.rotation, oldPlayerParentT.parent);

				Player newPlayer = newPlayerGO.GetComponentInChildren<Player>();

				AlignAndDestroy(oldPlayer.camFollowTarget, newPlayer.camFollowTarget);
				AlignAndDestroy(newPlayer.camFollowTarget.GetChild(0),
					oldPlayer.camFollowTarget.GetChild(0));

				newPlayer.camFollowTarget = oldPlayer.camFollowTarget;

				Destroy(oldPlayerParentT.gameObject);

				_onHeal.Invoke();
			}
		}

		void AlignAndDestroy(Transform oldT, Transform newT)
		{
			oldT.parent = newT.parent;
			oldT.position = newT.position;
			oldT.rotation = newT.rotation;

			Destroy(newT.gameObject);
		}

		private void OnDrawGizmos()
		{
			var col = Gizmos.color;
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(_spawnPosition.position, 1);
			Gizmos.color = col;
		}
	}
}