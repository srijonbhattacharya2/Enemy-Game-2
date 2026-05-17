using UnityEngine;
using UnityEngine.UI;

public class health_slider : MonoBehaviour
{
	private PlayerMovement playerScript;

	private RectTransform rect;

	private RawImage image;

	void Start()
	{
		rect = GetComponent<RectTransform>();

		image = GetComponent<RawImage>();

		GameObject playerObject =
			GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			playerScript =
				playerObject.GetComponent<PlayerMovement>();
		}
	}

	void Update()
	{
		float percentage =
			playerScript.Health * 10f;

		float width =
			(percentage / 100f) * 233.5782f;

		rect.sizeDelta =
			new Vector2(width, rect.sizeDelta.y);

		Color targetColor;

		if (playerScript.Health <= 2)
		{
			targetColor = Color.red;
		}
		else if (playerScript.Health <= 5)
		{
			targetColor = Color.yellow;
		}
		else
		{
			targetColor = Color.green;
		}

		image.color =
			Color.Lerp(
				image.color,
				targetColor,
				Time.deltaTime
			);
	}
}