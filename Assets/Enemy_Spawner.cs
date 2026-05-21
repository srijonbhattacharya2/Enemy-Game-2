using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private float minimumSpawnDelay = 0.1f;

	[SerializeField] private float delayDecreaseInterval = 3f;

	[SerializeField] private GameObject enemyPrefab1;

	[SerializeField] private GameObject enemyPrefab2;

	[SerializeField] private float spawnDelay = 2f;

	[SerializeField] private float decreaseAmount = 0.1f;

	[SerializeField] private int poolSize = 30;

	[SerializeField] private float offset = 2f;

	[SerializeField] private float boostInterval = 7f;

	[SerializeField] private int boostSpawnAmount = 5;

	[SerializeField] private float boostIncreaseInterval = 3f;

	[SerializeField] private int boostIncreaseAmount = 1;

	private List<GameObject> enemyPool =
		new List<GameObject>();

	void Start()
	{
		CreatePool();

		StartCoroutine(SpawnLoop());

		StartCoroutine(DecreaseSpawnDelay());

		StartCoroutine(SpawnBoostLoop());

		StartCoroutine(IncreaseBoostAmount());
	}

	void CreatePool()
	{
		int halfPool = poolSize / 2;

		for (int i = 0; i < halfPool; i++)
		{
			GameObject enemy1 = Instantiate(
				enemyPrefab1,
				Vector2.zero,
				Quaternion.identity
			);

			enemy1.SetActive(false);

			enemyPool.Add(enemy1);
		}

		for (int i = 0; i < poolSize - halfPool; i++)
		{
			GameObject enemy2 = Instantiate(
				enemyPrefab2,
				Vector2.zero,
				Quaternion.identity
			);

			enemy2.SetActive(false);

			enemyPool.Add(enemy2);
		}
	}

	IEnumerator SpawnLoop()
	{
		while (true)
		{
			SpawnEnemy();

			yield return new WaitForSeconds(
				spawnDelay
			);
		}
	}

	IEnumerator DecreaseSpawnDelay()
	{
		while (spawnDelay > minimumSpawnDelay)
		{
			yield return new WaitForSeconds(
				delayDecreaseInterval
			);

			spawnDelay -= decreaseAmount;

			if (spawnDelay < minimumSpawnDelay)
			{
				spawnDelay = minimumSpawnDelay;
			}
		}
	}

	IEnumerator SpawnBoostLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(
				boostInterval
			);

			for (
				int i = 0;
				i < boostSpawnAmount;
				i++
			)
			{
				SpawnEnemy();
			}
		}
	}

	IEnumerator IncreaseBoostAmount()
	{
		while (true)
		{
			yield return new WaitForSeconds(
				boostIncreaseInterval
			);

			boostSpawnAmount +=
				boostIncreaseAmount;
		}
	}

	void SpawnEnemy()
	{
		GameObject enemy =
			GetRandomInactiveEnemy();

		if (enemy != null)
		{
			enemy.transform.position =
				GetRandomOutsidePosition();

			enemy.SetActive(true);
		}
	}

	GameObject GetRandomInactiveEnemy()
	{
		List<GameObject> inactiveEnemies =
			new List<GameObject>();

		for (int i = 0; i < enemyPool.Count; i++)
		{
			if (!enemyPool[i].activeInHierarchy)
			{
				inactiveEnemies.Add(
					enemyPool[i]
				);
			}
		}

		if (inactiveEnemies.Count == 0)
		{
			return null;
		}

		return inactiveEnemies[
			Random.Range(
				0,
				inactiveEnemies.Count
			)
		];
	}

	Vector2 GetRandomOutsidePosition()
	{
		Camera cam = Camera.main;

		int side = Random.Range(0, 4);

		float screenLeft =
			cam.ViewportToWorldPoint(
				new Vector3(
					0,
					0,
					Mathf.Abs(
						cam.transform.position.z
					)
				)
			).x;

		float screenRight =
			cam.ViewportToWorldPoint(
				new Vector3(
					1,
					0,
					Mathf.Abs(
						cam.transform.position.z
					)
				)
			).x;

		float screenBottom =
			cam.ViewportToWorldPoint(
				new Vector3(
					0,
					0,
					Mathf.Abs(
						cam.transform.position.z
					)
				)
			).y;

		float screenTop =
			cam.ViewportToWorldPoint(
				new Vector3(
					0,
					1,
					Mathf.Abs(
						cam.transform.position.z
					)
				)
			).y;

		Vector2 spawnPos = Vector2.zero;

		switch (side)
		{
			case 0:

				spawnPos = new Vector2(
					screenLeft - offset,

					Random.Range(
						screenBottom,
						screenTop
					)
				);

				break;

			case 1:

				spawnPos = new Vector2(
					screenRight + offset,

					Random.Range(
						screenBottom,
						screenTop
					)
				);

				break;

			case 2:

				spawnPos = new Vector2(
					Random.Range(
						screenLeft,
						screenRight
					),

					screenBottom - offset
				);

				break;

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
}