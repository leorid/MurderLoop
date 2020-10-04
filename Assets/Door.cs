using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField] float _openDuration = 1;
	[SerializeField] bool _openOnStart;
	[SerializeField] Vector3 openOffset;

	[SerializeField] bool _wantOpen;
	Vector3 openPos;
	Vector3 closedPos;

	float normalized;

	private void Start()
	{
		closedPos = transform.position;
		openPos = transform.position + openOffset;
		_wantOpen = _openOnStart;
	}

	public void Open()
	{
		_wantOpen = true;
	}
	public void Close()
	{
		_wantOpen = false;
	}

	void Update()
    {
		if (_wantOpen)
		{
			normalized += Time.deltaTime / _openDuration;
		}
		else
		{
			normalized -= Time.deltaTime / _openDuration;
		}
		normalized = Mathf.Clamp01(normalized);

		transform.position = Vector3.Lerp(closedPos, openPos, normalized);
	}
}
