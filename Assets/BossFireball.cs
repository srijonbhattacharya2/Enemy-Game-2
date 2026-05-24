using UnityEngine;

public class BossFireball : MonoBehaviour
{	
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip hitSFX;
	
	public float speed = 6f;

	private Vector2 direction;

	public void SetDirection(Vector2 dir)
	{
		direction = dir.normalized;
	}

	void Update()
	{
		transform.position +=
			(Vector3)direction *
			speed *
			Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerMovement player =
				other.GetComponent<PlayerMovement>();

			if (player != null)
			{
				player.Health--;

				if (audioSource != null &&
					hitSFX != null)
				{
					audioSource.PlayOneShot(hitSFX);
				}
			}

			gameObject.SetActive(false);
		}
	}

	void OnBecameInvisible()
	{
		gameObject.SetActive(false);
	}
}