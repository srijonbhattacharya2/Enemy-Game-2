using UnityEngine;

public class JoystickCanvas : MonoBehaviour
{
	[SerializeField] private GameObject touchControls;

	private void Start()
	{
		// Disable controls if device is not touchscreen
		if (!Input.touchSupported)
		{
			touchControls.SetActive(false);
		}
	}
}