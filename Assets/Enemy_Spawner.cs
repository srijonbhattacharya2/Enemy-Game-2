using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;

	[SerializeField] private float spawnDelay = 2f;

	[SerializeField] private int maxEnemies = 20;

	// Distance outside screen
	[SerializeField] private float offset = 2f;

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
				Vector2 spawnPosition =
					GetRandomOutsidePosition();

				GameObject enemy = Instantiate(
					enemyPrefab,
					spawnPosition,
					Quaternion.identity
				);

				currentEnemies++;

				Enemy enemyScript =
					enemy.GetComponent<Enemy>();

				if (enemyScript != null)
				{
					enemyScript.spawner = this;
				}
			}

			yield return new WaitForSeconds(spawnDelay);
		}
	}

	Vector2 GetRandomOutsidePosition()
	{
		Camera cam = Camera.main;

		int side = Random.Range(0, 4);

		float screenLeft =
			cam.ViewportToWorldPoint(
				new Vector3(0, 0,
				Mathf.Abs(cam.transform.position.z))
			).x;

		float screenRight =
			cam.ViewportToWorldPoint(
				new Vector3(1, 0,
				Mathf.Abs(cam.transform.position.z))
			).x;

		float screenBottom =
			cam.ViewportToWorldPoint(
				new Vector3(0, 0,
				Mathf.Abs(cam.transform.position.z))
			).y;

		float screenTop =
			cam.ViewportToWorldPoint(
				new Vector3(0, 1,
				Mathf.Abs(cam.transform.position.z))
			).y;

		Vector2 spawnPos = Vector2.zero;

		switch (side)
		{
			// LEFT
			case 0:

				spawnPos = new Vector2(
					screenLeft - offset,

					Random.Range(
						screenBottom,
						screenTop
					)
				);

				break;

			// RIGHT
			case 1:

				spawnPos = new Vector2(
					screenRight + offset,

					Random.Range(
						screenBottom,
						screenTop
					)
				);

				break;

			// BOTTOM
			case 2:

				spawnPos = new Vector2(
					Random.Range(
						screenLeft,
						screenRight
					),

					screenBottom - offset
				);

				break;

			// TOP
			case 3:

				spawnPos = new Vector2(
					Random.Range(
						screenLeft,
						screenRight
					),

					screenTop + offset
				);

				break;
		}

		return spawnPos;
	}

	public void EnemyDestroyed()
	{
		currentEnemies--;
	}
}