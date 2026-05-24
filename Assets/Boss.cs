using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
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
		if (player != null)
		{
			Vector3 direction =
				(player.position -
				transform.position).normalized;

			if (moveBackwards)
			{
				direction = -direction;
			}

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
}