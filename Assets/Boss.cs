using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
	[SerializeField]
	private float separation = 2f;
	
	[SerializeField]
	private GameObject[] fireballs;

	[SerializeField]
	private float fireballCooldown = 2f;

	[SerializeField]
	private int fireballsPerShot = 1;

	[SerializeField]
	private float fireballIncreaseTime = 2f;

	private bool canShoot = true;
	
	private SpriteRenderer spriteRenderer;

	private Color originalColor;

	private Coroutine flashCoroutine;
	
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip hurtSFX;

	private bool canDamagePlayer = true;
	
	[SerializeField]
	private int hp = 50;

	[SerializeField]
	private float speed = 5f;

	private bool moveBackwards = false;

	private bool touchingPlayer = false;

	private bool hasBeenVisible = false;

	private Transform player;

	private PlayerMovement playerScript;

	void Start()
	{
		spriteRenderer =
			GetComponent<SpriteRenderer>();

		originalColor =
			spriteRenderer.color;

		StartCoroutine(
			IncreaseFireballsOverTime()
		);
		
		GameObject playerObject =
			GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			player = playerObject.transform;

			playerScript =
				playerObject.GetComponent<PlayerMovement>();
		}
	}

	void Update()
	{
		if (canShoot)
		{
			StartCoroutine(ShootFireball());
		}
		
		if (player != null)
		{
			Vector3 direction =
				(player.position -
				transform.position).normalized;

			GameObject[] enemies =
				GameObject.FindGameObjectsWithTag(
					"Enemy"
				);

			foreach (GameObject enemy in enemies)
			{
				float distance =
					Vector2.Distance(
						transform.position,
						enemy.transform.position
					);

				if (distance < separation)
				{
					Vector3 avoidDirection =
						(transform.position -
						enemy.transform.position)
						.normalized;

					direction += avoidDirection;
				}
			}

			if (moveBackwards)
			{
				direction = -direction;
			}

			direction.Normalize();
			
			transform.position +=
				direction *
				speed *
				Time.deltaTime;
		}
	}

	public void TakeDamage(int damage)
	{
		hp -= damage;

		if (flashCoroutine != null)
		{
			StopCoroutine(flashCoroutine);
		}

		flashCoroutine =
			StartCoroutine(FlashRed());

		Debug.Log("Boss HP: " + hp);

		if (hp <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Debug.Log("Boss Defeated!");

		foreach (GameObject fireball in fireballs)
		{
			fireball.SetActive(false);
		}

		gameObject.SetActive(false);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") &&
			canDamagePlayer)
		{
			StartCoroutine(HitPlayer());
		}
	}

	IEnumerator StopMovingBackwards()
	{
		yield return new WaitForSeconds(0.3f);

		speed /= 1.5f;

		moveBackwards = false;

		yield return new WaitForSeconds(0.2f);

		touchingPlayer = false;
	}

	void OnBecameVisible()
	{
		hasBeenVisible = true;
	}

	void OnBecameInvisible()
	{
		if (hasBeenVisible)
		{
			gameObject.SetActive(false);
		}
	}

	IEnumerator HitPlayer()
	{
		canDamagePlayer = false;

		moveBackwards = true;

		if (playerScript != null)
		{
			playerScript.Health--;

			Debug.Log(playerScript.Health);
		}

		if (audioSource != null &&
			hurtSFX != null)
		{
			audioSource.PlayOneShot(hurtSFX);
		}

		yield return new WaitForSeconds(0.2f);

		moveBackwards = false;

		yield return new WaitForSeconds(0.5f);

		canDamagePlayer = true;
	}

	IEnumerator FlashRed()
	{
		spriteRenderer.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		spriteRenderer.color = originalColor;
	}

	IEnumerator ShootFireball()
	{
		canShoot = false;

		for (int i = 0;
			i < fireballsPerShot;
			i++)
		{
			GameObject fireball = null;

			foreach (GameObject fb in fireballs)
			{
				if (!fb.activeInHierarchy)
				{
					fireball = fb;

					break;
				}
			}

			if (fireball != null)
			{
				fireball.SetActive(true);

				fireball.transform.position =
					transform.position;

				Vector2 direction =
					(player.position -
					fireball.transform.position)
					.normalized;

				direction +=
					Random.insideUnitCircle * 0.7f;

				BossFireball fireballScript =
					fireball.GetComponent<BossFireball>();

				fireballScript.SetDirection(direction);
			}
		}

		yield return new WaitForSeconds(
			fireballCooldown
		);

		canShoot = true;
	}

	IEnumerator IncreaseFireballsOverTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(
				fireballIncreaseTime
			);

			fireballsPerShot++;

			Debug.Log(
				"Fireballs Per Shot: " +
				fireballsPerShot
			);
		}
	}

	void OnEnable()
	{
		spriteRenderer.color = originalColor;
		
		canShoot = true;

		canDamagePlayer = true;

		moveBackwards = false;
	}
}