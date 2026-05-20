using UnityEngine;

public class EnemySwarm : MonoBehaviour
{

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip hurt;
   	private PlayerMovement playerScript;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerScript.Health--;

			Debug.Log(playerScript.Health);

			audioSource.PlayOneShot(hurt);
		}
	}
}
