using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
	[HideInInspector]
	public EnemySpawner spawner;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip hurt;
	private float speed;

	private bool hasBeenVisible = false;

	private bool movingBackwards = false;

	private Transform player;
	private PlayerMovement playerScript;

	void Start()
	{
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			player = playerObject.transform;

			playerScript =
				playerObject.GetComponent<PlayerMovement>();
		}

		speed = Random.Range(1f, 5f);
	}
	
	void Update()
	{
		if (player != null)
		{
			Vector3 direction =
				(player.position - transform.position).normalized;

			if (movingBackwards)
			{
				direction = -direction;
			}

			transform.position += direction * speed * Time.deltaTime;
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
			if (spawner != null)
			{
				spawner.EnemyDestroyed();
			}

			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			playerScript.Health--;
			Debug.Log (playerScript.Health);
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