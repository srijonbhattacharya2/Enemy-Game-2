using UnityEngine;
using System.Collections;

public class EnemySwarm_enemy_children : MonoBehaviour
{
	private bool canDamage = true;
    
    [HideInInspector]
	public EnemySpawner spawner;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip hurt;

	[SerializeField] private GameObject thingToHide;

	private float speed;

	[SerializeField] private float minSPEED;
	[SerializeField] private float maxSPEED;

	[SerializeField] private float separation;

	private bool movingBackwards = false;

	private Transform player;
	private PlayerMovement playerScript;

	void Start()
	{
		GameObject playerObject =
			GameObject.FindGameObjectWithTag("Player");

		if (playerObject != null)
		{
			player = playerObject.transform;

			playerScript =
				playerObject.GetComponent<PlayerMovement>();
		}

		speed = Random.Range(minSPEED, maxSPEED);
	}

    void Update()
    {
        // Hide/show object based on player health
        if (thingToHide != null && playerScript != null)
        {
            thingToHide.SetActive(playerScript.Health > 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            canDamage = false;

            playerScript.Health--;

			GetComponentInParent<EnemySwarm>()
				.StartCoroutine("MoveBackwards");

            Debug.Log(playerScript.Health);

            StartCoroutine(MoveBackwards());

            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.5f);

        canDamage = true;
    }

	IEnumerator MoveBackwards()
	{
		audioSource.PlayOneShot(hurt);

		movingBackwards = true;

		speed *= 1.3f;

		yield return new WaitForSeconds(0.3f);

		speed /= 1.3f;

		movingBackwards = false;
	}
}