using UnityEngine;
using System.Collections;

public class EnemySwarm : MonoBehaviour
{
	private bool hasBeenVisible = false;

	private Transform player;

	[SerializeField] private float separation;

	private float speed;

	[SerializeField] private float minSpeed;
	[SerializeField] private float maxSpeed;

	private bool movingBackwards = false;

	private GameObject[] allChildren;

	private void Start()
	{
		speed = Random.Range(minSpeed, maxSpeed);

		GameObject playerObject =
			GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			player = playerObject.transform;
		}

		allChildren =
			new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
		{
			allChildren[i] =
				transform.GetChild(i).gameObject;
		}
	}

	private void Update()
	{
		if (player != null)
		{
			Vector2 direction =
				(player.position - transform.position).normalized;

			GameObject[] swarms =
				GameObject.FindGameObjectsWithTag("EnemySwarm");

			foreach (GameObject swarm in swarms)
			{
				if (swarm == gameObject)
				{
					continue;
				}

				float distance =
					Vector2.Distance(
						transform.position,
						swarm.transform.position
					);

				if (distance < separation)
				{
					Vector2 avoidDirection =
						(transform.position -
						swarm.transform.position).normalized;

					direction += avoidDirection;
				}
			}

			direction.Normalize();

			float angle =
				Mathf.Atan2(direction.y, direction.x) *
				Mathf.Rad2Deg;

			Quaternion targetRotation =
				Quaternion.Euler(0f, 0f, angle - 90);

			transform.rotation =
				Quaternion.Lerp(
					transform.rotation,
					targetRotation,
					10f * Time.deltaTime
				);

			if (movingBackwards)
			{
				direction = -direction;
			}

			transform.position = Vector2.MoveTowards(
				transform.position,
				transform.position +
				(Vector3)direction,
				speed * Time.deltaTime
			);
		}
	}

	public IEnumerator MoveBackwards()
	{
		movingBackwards = true;

		yield return new WaitForSeconds(0.3f);

		movingBackwards = false;
	}

	private void OnEnable()
	{
		hasBeenVisible = false;

		if (allChildren != null)
		{
			for (int i = 0; i < allChildren.Length; i++)
			{
				allChildren[i].SetActive(true);
			}
		}
	}

	private void LateUpdate()
	{
		int aliveChildren = 0;

		Transform lastChild = null;

		bool anyVisible = false;

		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child =
				transform.GetChild(i);

			if (child.gameObject.activeSelf)
			{
				aliveChildren++;

				lastChild = child;
			}

			Renderer childRenderer =
				child.GetComponent<Renderer>();

			if (
				childRenderer != null &&
				childRenderer.isVisible
			)
			{
				anyVisible = true;

				hasBeenVisible = true;
			}
		}

		// No children alive
		if (aliveChildren == 0)
		{
			gameObject.SetActive(false);

			return;
		}

		// One child alive
		if (aliveChildren == 1 && lastChild != null)
		{
			lastChild.rotation =
				Quaternion.Euler(0f, 0f, 90f);
		}

		// Entire swarm left screen
		if (hasBeenVisible && !anyVisible)
		{
			gameObject.SetActive(false);

			return;
		}

		// Too far from player
		if (player != null)
		{
			float distanceFromPlayer =
				Vector2.Distance(
					transform.position,
					player.position
				);

			if (distanceFromPlayer > 30f)
			{
				gameObject.SetActive(false);
			}
		}
	}
}