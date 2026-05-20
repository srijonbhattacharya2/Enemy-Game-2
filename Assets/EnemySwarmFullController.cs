using UnityEngine;

public class EnemySwarmController : MonoBehaviour
{
	[SerializeField] private float speed1 = 2f;
	[SerializeField] private float speed2 = 5f;

	[SerializeField] private float childCheckDelay = 0.5f;

	private Transform player;

	private float moveSpeed;

	private float childCheckTimer;

	private bool singleChildMode;

	private void Start()
	{
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			player = playerObject.transform;
		}

		moveSpeed = Random.Range(0, 2) == 0 ? speed1 : speed2;
	}

	private void Update()
	{
		CheckChildrenLogic();

		if (player == null)
		{
			return;
		}

		Vector2 direction = (player.position - transform.position).normalized;

		float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 45;

		if (singleChildMode == true)
		{
			angle = 90f;
		}

		transform.rotation = Quaternion.Euler(0f, 0f, angle);

		transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
	}

	private void CheckChildrenLogic()
	{
		childCheckTimer += Time.deltaTime;

		if (childCheckTimer < childCheckDelay)
		{
			return;
		}

		childCheckTimer = 0f;

		int activeChildren = 0;

		// Only direct children
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf)
			{
				activeChildren++;
			}
		}

		// No children alive
		if (activeChildren == 0)
		{
			gameObject.SetActive(false);

			return;
		}

		// One child alive
		singleChildMode = activeChildren == 1;
	}
}