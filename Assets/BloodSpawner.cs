using JL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawner : MonoBehaviour
{
	[SerializeField] float _duration = 3;
	[SerializeField] float _startSpeed = 3;
	[SerializeField] int _amount = 30;

	Transform bloodParent;

	public void InitBlood(CharacterTop characterTop)
	{
		bloodParent = characterTop.transform;
	}

	void OnEnable()
	{
		if (Application.isPlaying) StartCoroutine(SpawnIt());
	}

	IEnumerator SpawnIt()
	{
		float timer = 0;

		int spawnedAmount = 0;

		while (timer < _duration)
		{
			timer += Time.deltaTime;

			int currentAmount = (int)Mathf.Lerp(0, _amount, timer / _duration);

			while (currentAmount > spawnedAmount && BloodPool.pool.Count > 0)
			{
				spawnedAmount++;
				Rigidbody rb = BloodPool.pool.Dequeue();
				rb.transform.position = transform.position;
				rb.transform.rotation = Random.rotation;
				rb.transform.SetParent(bloodParent, true);
				rb.gameObject.SetActive(true);
				rb.velocity = transform.right * _startSpeed;
			}

			yield return null;
		}
	}
}
