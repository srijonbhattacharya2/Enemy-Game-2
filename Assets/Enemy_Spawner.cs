using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;

	[SerializeField] private Transform[] spawnPoints;

	[SerializeField] private float spawnDelay = 2f;

	[SerializeField] private int maxEnemies = 20;

	private int currentEnemies = 0;

	void Start()
	{
		StartCoroutine(SpawnLoop());
	}

	IEnumerator SpawnLoop()
	{
		while (true)
		{
			if (currentEnemies < maxEnemies)
			{
				int randomIndex = Random.Range(0, spawnPoints.Length);

				Transform spawnPoint = spawnPoints[randomIndex];

				GameObject enemy = Instantiate(
					enemyPrefab,
					spawnPoint.position,
					Quaternion.identity
				);

				currentEnemies++;

				Enemy enemyScript = enemy.GetComponent<Enemy>();

				if (enemyScript != null)
				{
					enemyScript.spawner = this;
				}
			}

			yield return new WaitForSeconds(spawnDelay);
		}
	}

	public void EnemyDestroyed()
	{
		currentEnemies--;
	}
}