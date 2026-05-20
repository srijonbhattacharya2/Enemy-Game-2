using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timerText;

	private float timer;

	private void Update()
	{
		timer += Time.deltaTime;

		int hours = Mathf.FloorToInt(timer / 3600);
		int minutes = Mathf.FloorToInt((timer % 3600) / 60);
		int seconds = Mathf.FloorToInt(timer % 60);

		if (hours > 0)
		{
			timerText.text =
				hours.ToString("00") + ":" +
				minutes.ToString("00") + ":" +
				seconds.ToString("00");
		}
		else
		{
			timerText.text =
				minutes.ToString("00") + ":" +
				seconds.ToString("00");
		}
	}
}