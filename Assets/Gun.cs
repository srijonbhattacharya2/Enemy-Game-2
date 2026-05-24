using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
	private bool bulletExists = false;

	public GameObject bulletPrefab;

	public float bulletSpeed = 10f;

	public MovementManager movementManager;

	public WeaponButtonManager weaponButtonManager;

	private List<GameObject> bulletPool =
		new List<GameObject>();

	public int poolSize = 10;

	void Start()
	{
		for (int i = 0; i < poolSize; i++)
		{
			GameObject bullet =
				Instantiate(
					bulletPrefab,
					Vector3.zero,
					Quaternion.identity
				);

			bullet.SetActive(false);

			bulletPool.Add(bullet);
		}
	}

	void Update()
	{
		transform.rotation =
			Quaternion.Euler(
				0f,
				0f,
				movementManager.targetAngle
			);

		if (weaponButtonManager.shoot)
		{
			Shoot();

			weaponButtonManager.shoot = false;
		}
	}

	void Shoot()
	{
		if (bulletExists)
		{
			return;
		}

		GameObject nearestEnemy =
			FindNearestEnemy();

		if (nearestEnemy == null)
		{
			return;
		}

		GameObject bullet =
			GetPooledBullet();

		if (bullet == null)
		{
			return;
		}

		bullet.SetActive(true);

		bullet.transform.position =
			transform.position;

		bullet.transform.rotation =
			Quaternion.identity;

		Vector2 direction =
			(
				nearestEnemy.transform.position -
				transform.position
			).normalized;

		bullet.transform.right =
			-direction;

		Rigidbody2D rb =
			bullet.GetComponent<Rigidbody2D>();

		rb.linearVelocity =
			direction * bulletSpeed;

		Bullet bulletScript =
			bullet.GetComponent<Bullet>();

		bulletScript.StartLifeTimer();

		bulletExists = true;

		StartCoroutine(
			ResetBullet()
		);
	}

	GameObject GetPooledBullet()
	{
		foreach (GameObject bullet in bulletPool)
		{
			if (!bullet.activeInHierarchy)
			{
				return bullet;
			}
		}

		return null;
	}

	IEnumerator ResetBullet()
	{
		yield return new WaitForSeconds(0.3f);

		bulletExists = false;
	}

	GameObject FindNearestEnemy()
	{
		GameObject[] enemies =
			GameObject.FindGameObjectsWithTag(
				"Enemy"
			);

		GameObject[] bosses =
			GameObject.FindGameObjectsWithTag(
				"Boss"
			);

		GameObject nearestEnemy = null;

		float shortestDistance = Mathf.Infinity;

		foreach (GameObject enemy in enemies)
		{
			float distance =
				Vector2.Distance(
					transform.position,
					enemy.transform.position
				);

			if (distance < shortestDistance)
			{
				shortestDistance = distance;

				nearestEnemy = enemy;
			}
		}

		foreach (GameObject boss in bosses)
		{
			float distance =
				Vector2.Distance(
					transform.position,
					boss.transform.position
				);

			if (distance < shortestDistance)
			{
				shortestDistance = distance;

				nearestEnemy = boss;
			}
		}

		return nearestEnemy;
	}
}