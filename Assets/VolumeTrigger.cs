using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeTrigger : MonoBehaviour
{
	[SerializeField] UnityEvent _unityEvent;
	[SerializeField] bool _once;

	bool _alreadyTriggered;

	private void OnTriggerEnter(Collider other)
	{
		if (_once && _alreadyTriggered) return;

		if (other.CompareTag("Player"))
		{
			_unityEvent.Invoke();
			_alreadyTriggered = true;
		}
	}
}
