using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;

	[SerializeField] private float spawnDelay = 2f;

	[SerializeField] private int poolSize = 30;

	[SerializeField] private float offset = 2f;

	private List<GameObject> enemyPool =
		new List<GameObject>();

	void Start()
	{
		CreatePool();

		StartCoroutine(SpawnLoop());
	}

	void CreatePool()
	{
		for (int i = 0; i < poolSize; i++)
		{
			GameObject enemy = Instantiate(
				enemyPrefab,
				Vector2.zero,
				Quaternion.identity
			);

			enemy.SetActive(false);

			enemyPool.Add(enemy);
		}
	}

	IEnumerator SpawnLoop()
	{
		while (true)
		{
			GameObject enemy = GetInactiveEnemy();

			if (enemy != null)
			{
				enemy.transform.position =
					GetRandomOutsidePosition();

				enemy.SetActive(true);
			}

			yield return new WaitForSeconds(
				spawnDelay
			);
		}
	}

	GameObject GetInactiveEnemy()
	{
		for (int i = 0; i < enemyPool.Count; i++)
		{
			if (!enemyPool[i].activeInHierarchy)
			{
				return enemyPool[i];
			}
		}

		return null;
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