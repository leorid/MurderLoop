using JL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadDrop : MonoBehaviour
{
	[SerializeField] GameObject particlePrefab;
	Queue<GameObject> _particlePool = new Queue<GameObject>();
	float particleDuration = 1;
	[SerializeField] TMPro.TextMeshPro textMesh;
	public int counter = 10;

	[SerializeField] UnityEvent unityEvent;

	// Start is called before the first frame update
	void Start()
	{
		//get particle duration
		particleDuration = particlePrefab.GetComponent<ParticleSystem>().main.duration;
		textMesh.text = counter.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out HeadHealth head))
		{
			Trigger(head);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent(out HeadHealth head))
		{
			Trigger(head);
		}
	}

	void Trigger(HeadHealth head)
	{
		if (head.value < counter) return;

		unityEvent.Invoke();

		SpawnParticle(head.transform.position);

		if (head.value == counter)
		{
			Destroy(head.gameObject);
		}
		else
		{
			head.value -= counter;
			head.UpdateText();
		}

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		textMesh.gameObject.SetActive(false);
		enabled = false;
	}

	void SpawnParticle(Vector3 position)
	{
		GameObject go;
		if (_particlePool.Count > 0)
		{
			go = _particlePool.Dequeue();
			go.transform.position = position;
			go.SetActive(true);
		}
		else
		{
			go = Instantiate(particlePrefab);
			go.transform.position = position;
		}
		ReturnToPoolAfterSeconds(go, particleDuration);
	}

	IEnumerator ReturnToPoolAfterSeconds(GameObject go, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		go.SetActive(false);
		_particlePool.Enqueue(go);
	}
}
