using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
	public static Transform respawnPosition;
	[SerializeField] Material _clamedMat;
	[SerializeField] Material _openMat;

	[SerializeField] UnityEvent _OnClaim;
	[SerializeField] UnityEvent _OnRespawn;

	delegate void Claimed();
	static event Claimed _OnClaimed;

	MeshRenderer _renderer;

	private void Start()
	{
		_renderer = GetComponent<MeshRenderer>();
		_OnClaimed += Checkpoint_OnClaimed;
	}

	private void Checkpoint_OnClaimed()
	{
		if(respawnPosition == transform)
		{
			_renderer.sharedMaterial = _clamedMat;
		}
		else
		{
			_renderer.sharedMaterial = _openMat;
		}
	}

	public void OnRespawn()
	{
		_OnRespawn.Invoke();
	}

	public void SetCheckpoint()
	{
		respawnPosition = transform;
		_OnClaim.Invoke();
		_OnClaimed?.Invoke();
	}
}
