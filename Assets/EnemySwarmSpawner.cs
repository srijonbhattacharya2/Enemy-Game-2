using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySwarmSpawner : MonoBehaviour
{
	[SerializeField] private GameObject swarmPrefab;

	[SerializeField] private int poolSize = 10;

	[SerializeField] private float spawnDelay = 7f;

	[SerializeField] private float offset = 2f;

	private List<GameObject> swarmPool =
		new List<GameObject>();

	private void Start()
	{
		CreatePool();

		StartCoroutine(SpawnLoop());
	}

	private void CreatePool()
	{
		for (int i = 0; i < poolSize; i++)
		{
			GameObject swarm = Instantiate(
				swarmPrefab,
				Vector2.zero,
				Quaternion.identity
			);

			swarm.SetActive(false);

			swarmPool.Add(swarm);
		}
	}

	private IEnumerator SpawnLoop()
	{
		while (true)
		{
			GameObject swarm = GetInactiveSwarm();

			if (swarm != null)
			{
				swarm.transform.position =
					GetRandomOutsidePosition();

				swarm.SetActive(true);
			}

			yield return new WaitForSeconds(
				spawnDelay
			);
		}
	}

	private GameObject GetInactiveSwarm()
	{
		for (int i = 0; i < swarmPool.Count; i++)
		{
			if (!swarmPool[i].activeInHierarchy)
			{
				return swarmPool[i];
			}
		}

		return null;
	}

	private Vector2 GetRandomOutsidePosition()
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