using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject bossPrefab;

	[SerializeField]
	private int poolSize = 1;

	[SerializeField]
	private float spawnDelay = 30f;

	[SerializeField]
	private float offset = 2f;

	private List<GameObject> bossPool =
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
			GameObject boss =
				Instantiate(
					bossPrefab,
					Vector2.zero,
					Quaternion.identity
				);

			boss.SetActive(false);

			bossPool.Add(boss);
		}
	}

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                spawnDelay
            );

            GameObject boss =
                GetInactiveBoss();

            if (boss != null)
            {
                boss.transform.position =
                    GetRandomOutsidePosition();

                boss.SetActive(true);
            }
        }
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

	GameObject GetInactiveBoss()
	{
		for (int i = 0; i < bossPool.Count; i++)
		{
			if (!bossPool[i].activeInHierarchy)
			{
				return bossPool[i];
			}
		}

		return null;
	}
}