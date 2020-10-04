using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnAwake : MonoBehaviour
{
	[SerializeField] GameObject _objectToEnable;

	private void Awake()
	{
		if(_objectToEnable) _objectToEnable.SetActive(true);
	}
}
