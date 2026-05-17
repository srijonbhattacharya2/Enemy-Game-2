using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class game_over_panel : MonoBehaviour
{
	private PlayerMovement playerScript;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip sfx;

	private bool gameOverTriggered = false;

	void Start()
	{
		GameObject playerObject =
			GameObject.FindGameObjectWithTag("Player");

		playerScript =
			playerObject.GetComponent<PlayerMovement>();
	}

	void Update()
	{
		if (playerScript.Health <= 0 && !gameOverTriggered)
		{
			gameOverTriggered = true;

			transform.Find("Game Over Panel")
				.gameObject.SetActive(true);

			audioSource.PlayOneShot(sfx);

			Time.timeScale = 0f;
		}
	}

	public void Restart_button_clicked()
	{Time.timeScale = 1f;

		SceneManager.LoadScene(
	SceneManager.GetActiveScene().buildIndex
);
	}
}