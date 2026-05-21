using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	[HideInInspector]
	public EnemySpawner spawner;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip hurt;

	[SerializeField] private GameObject thingToHide;

	private float speed;

	[SerializeField] private float minSPEED;
	[SerializeField] private float maxSPEED;

	[SerializeField] private float separation;

	private bool hasBeenVisible = false;
	private bool movingBackwards = false;

	private Transform player;
	private PlayerMovement playerScript;

	void Start()
	{
		GameObject playerObject =
			GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			player = playerObject.transform;

			playerScript =
				playerObject.GetComponent<PlayerMovement>();
		}

		speed = Random.Range(minSPEED, maxSPEED);
	}

	void Update()
	{
		// Hide/show object based on player health
		if (thingToHide != null && playerScript != null)
		{
			thingToHide.SetActive(playerScript.Health > 0);
		}

		if (player != null)
		{
			Vector3 direction =
				(player.position - transform.position).normalized;

			GameObject[] enemies =
				GameObject.FindGameObjectsWithTag("Enemy");

			foreach (GameObject enemy in enemies)
			{
				if (enemy == gameObject)
				{
					continue;
				}

				float distance =
					Vector2.Distance(
						transform.position,
						enemy.transform.position
					);

				if (distance < separation)
				{
					Vector3 avoidDirection =
						(transform.position -
						enemy.transform.position).normalized;

					direction += avoidDirection;
				}
			}

			direction.Normalize();

			if (movingBackwards)
			{
				direction = -direction;
			}

			transform.position = Vector3.Lerp(
				transform.position,
				transform.position +
				direction * speed * Time.deltaTime,
				200f * Time.deltaTime
			);
		}
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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerScript.Health--;

			Debug.Log(playerScript.Health);

			StartCoroutine(MoveBackwards());
		}
	}

	IEnumerator MoveBackwards()
	{
		audioSource.PlayOneShot(hurt);

		movingBackwards = true;

		speed *= 1.3f;

		yield return new WaitForSeconds(0.3f);

		speed /= 1.3f;

		movingBackwards = false;
	}
}