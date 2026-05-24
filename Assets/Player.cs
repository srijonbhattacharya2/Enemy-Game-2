using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float speed = 20f;

	private Rigidbody2D rb;
	private Vector2 movement;

	public int Health;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		movement = new Vector2(moveX, moveY);
	}

	void FixedUpdate()
	{
		rb.MovePosition(
			rb.position + movement.normalized * speed * Time.fixedDeltaTime
		);
	}
}