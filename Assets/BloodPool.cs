using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPool : MonoBehaviour
{
	public static Queue<Rigidbody> pool = new Queue<Rigidbody>();
	[SerializeField] Rigidbody bloodPrefab;
	[SerializeField] int _amount = 1000;
	private void Update()
	{
		if(pool.Count < _amount)
		{
			Rigidbody instance = Instantiate(bloodPrefab);
			instance.transform.SetParent(transform);
			instance.gameObject.SetActive(false);
			instance.transform.localScale *= Random.Range(0.9f, 1.1f);
			pool.Enqueue(instance);
		}
	}
}
