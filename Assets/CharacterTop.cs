using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class CharacterTop : MonoBehaviour
	{
		private void Start()
		{
			foreach(BloodSpawner bs in GetComponentsInChildren<BloodSpawner>(true))
			{
				bs.InitBlood(this);
			}
			foreach (BodyPart bp in GetComponentsInChildren<BodyPart>(true))
			{
				bp.InitBodyPart(this);
			}
		}
	}
}
