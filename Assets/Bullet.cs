using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float lifeTime = 3f;

	private Coroutine lifeCoroutine;

	public void StartLifeTimer()
	{
		if (lifeCoroutine != null)
		{
			StopCoroutine(lifeCoroutine);
		}

		lifeCoroutine =
			StartCoroutine(
				LifeTimer()
			);
	}

	IEnumerator LifeTimer()
	{
		yield return new WaitForSeconds(lifeTime);

		DeactivateBullet();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.gameObject.SetActive(false);
		}
	}

	void OnBecameInvisible()
	{
		DeactivateBullet();
	}

	void DeactivateBullet()
	{
		Rigidbody2D rb =
			GetComponent<Rigidbody2D>();

		rb.linearVelocity =
			Vector2.zero;

		gameObject.SetActive(false);
	}
}