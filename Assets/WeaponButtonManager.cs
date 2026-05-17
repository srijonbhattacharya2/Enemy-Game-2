using UnityEngine;
using System.Collections;

public class WeaponButtonManager : MonoBehaviour
{
	public bool shoot = false;

	public void weapon_button_clicked()
	{
		shoot = true;
		Debug.Log ("SHOOTING!!");

		StartCoroutine(Wait());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.1f);

		shoot = false;
	}
}