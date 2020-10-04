using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] GameObject _enemyPrefab;
	List<Transform> _spawnPoints = new List<Transform>();

	List<GameObject> _spawnedEnemies = new List<GameObject>();

	bool _spawned;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			if (child.gameObject.activeInHierarchy)
			{
				_spawnPoints.Add(child);
			}
		}
	}

	public void Spawn()
	{
		if (_spawned) return;
		_spawned = true;
		foreach (Transform t in _spawnPoints)
		{
			if (!t || !t.gameObject || !t.gameObject.activeInHierarchy) continue;
			SpawnEnemy(t.position);
		}
	}

	void SpawnEnemy(Vector3 position)
	{
		GameObject go = Instantiate(_enemyPrefab, position,
			_enemyPrefab.transform.rotation, transform);
		_spawnedEnemies.Add(go);
	}

	public void Clear()
	{
		_spawned = false;
		foreach (GameObject go in _spawnedEnemies)
		{
			if (go) Destroy(go);
		}
		_spawnedEnemies.Clear();
	}

	private void OnDrawGizmos()
	{
		var col = Gizmos.color;
		Gizmos.color = Color.red;
		foreach (Transform t in transform)
		{
			if (!t || !t.gameObject || !t.gameObject.activeInHierarchy) continue;
			Gizmos.DrawSphere(t.position, 1);
		}
		Gizmos.color = col;
	}
}
