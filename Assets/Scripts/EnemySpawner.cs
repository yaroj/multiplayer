using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
	public GameObject enemyPrefab;
	public float spawnInterval = 5f;
	public float spawnRange = 10f;
	public float spawnHeight = 1f;

	private void Start()
	{
			InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
	}

	void SpawnEnemy()
	{
		if(!IsServer) return;
		print("Spawning enemy...");
		Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), spawnHeight, Random.Range(-spawnRange, spawnRange));
		var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
		enemy.GetComponent<NetworkObject>().Spawn();
	}
}
