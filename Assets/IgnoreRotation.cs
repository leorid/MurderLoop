using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class IgnoreRotation : MonoBehaviour
	{
		void Update()
		{
			transform.rotation = Quaternion.identity;
		}
		void FixedUpdate()
		{
			transform.rotation = Quaternion.identity;
		}
		void LateUpdate()
		{
			transform.rotation = Quaternion.identity;
		}
	}
}
