using UnityEngine;

public class CameraBounds : MonoBehaviour
{
	public Transform target;

	public BoxCollider2D topBound;
	public BoxCollider2D bottomBound;
	public BoxCollider2D leftBound;
	public BoxCollider2D rightBound;

	private Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	void LateUpdate()
	{
		if (target == null) return;

		float camHeight = cam.orthographicSize;
		float camWidth = camHeight * cam.aspect;

		float minX = leftBound.bounds.max.x + camWidth;
		float maxX = rightBound.bounds.min.x - camWidth;

		float minY = bottomBound.bounds.max.y + camHeight;
		float maxY = topBound.bounds.min.y - camHeight;

		float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
		float clampedY = Mathf.Clamp(target.position.y, minY, maxY);

		transform.position = new Vector3(
			clampedX,
			clampedY,
			transform.position.z
		);
	}
}