using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	private bool bulletExists = false;
    
    public GameObject bulletPrefab;

	public float bulletSpeed = 10f;

	public MovementManager movementManager;

	public WeaponButtonManager weaponButtonManager;

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
                Instantiate(
                    bulletPrefab,
                    transform.position,
                    Quaternion.identity
                );

            bulletExists = true;

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

            Destroy(
                bullet,
                3f
            );

            StartCoroutine(
                ResetBullet()
            );
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

            return nearestEnemy;
        }
}