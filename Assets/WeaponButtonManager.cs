using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponButtonManager : MonoBehaviour
{
	public bool shoot = false;
	[SerializeField] private Image WeaponButton;

	private void Start()
	{
		Vibration.Init();
	}
	
	public void weapon_button_clicked()
	{
		shoot = true;
		Vibration.VibrateAndroid(50);
		Debug.Log ("SHOOTING!!");

		StartCoroutine(Wait());
	}

	void Update()
	{
        if (!shoot)
        {
		    Color color = WeaponButton.color;
			color.a = 0.4f;
			WeaponButton.color = color;
        }
        else
        {
		    Color color = WeaponButton.color;
			color.a = 0.7f;
			WeaponButton.color = color;
        }
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.1f);

		shoot = false;
	}
}