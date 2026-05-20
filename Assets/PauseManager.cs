using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
	[SerializeField] private Image buttonImage;

	[SerializeField] private Sprite playSprite;
	[SerializeField] private Sprite pauseSprite;

	private bool paused = false;

	public void TogglePause()
	{
		paused = !paused;

		if (paused)
		{
			Time.timeScale = 0f;
			buttonImage.sprite = playSprite;
		}
		else
		{
			Time.timeScale = 1f;
			buttonImage.sprite = pauseSprite;
		}
	}
}