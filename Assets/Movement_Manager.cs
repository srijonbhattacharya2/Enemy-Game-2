using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovementManager : MonoBehaviour
{
	[SerializeField] private Image JoystickBackground;
    [SerializeField] private Image JoystickHandle;
    
    public WeaponButtonManager weaponButtonManager;
    // [00:03:37] References for the variable joystick and player
	public VariableJoystick joystick;
	public Transform player;
	public float targetAngle;
	
	// [00:05:10] Movement speed multiplier
	private float moveSpeed = 6;

	private Vector2 joystickInputs;
	private Vector2 screenBounds;

	// Real joystick input
	private Vector2 realJoystickInputs;

	void Start()
	{
		// [00:07:30] Calculate the screen bounds based on the main camera to clamp position
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
	}

	void Update()
    {
        // [00:03:59] Get directional input from the joystick on Update
		joystickInputs = joystick.Direction;

        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is UnityEngine.InputSystem.Controls.ButtonControl button)
                {
                    if (button.isPressed)
                    {
                        weaponButtonManager.weapon_button_clicked();
                        break;
                    }
                }
            }
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
        {
            weaponButtonManager.weapon_button_clicked();
        }

		// Combine inputs
		Vector2 finalInputs;

		bool hasVirtualInput = joystickInputs != Vector2.zero;
		bool hasRealInput = realJoystickInputs != Vector2.zero;

        if (!hasVirtualInput)
        {
		    Color color = JoystickBackground.color;
			color.a = 0.4f;
			JoystickBackground.color = color;
            JoystickHandle.color = color;
        }
        else
        {
		    Color color = JoystickBackground.color;
			color.a = 0.7f;
			JoystickBackground.color = color;
            JoystickHandle.color = color;
        }

		// If both are moving, average them
		if (hasVirtualInput && hasRealInput)
		{
			finalInputs = (joystickInputs + realJoystickInputs) / 2f;
		}
		else if (hasVirtualInput)
		{
			finalInputs = joystickInputs;
		}
		else
		{
			finalInputs = realJoystickInputs;
		}

		// Save final movement input
		joystickInputs = finalInputs;

		// [00:05:53] Handle character rotation based on movement direction
		if (joystickInputs != Vector2.zero)
		{
			targetAngle = Mathf.Atan2(joystickInputs.x, joystickInputs.y) * Mathf.Rad2Deg; // NEEDY - for Gun!
			player.eulerAngles = new Vector3(0, 0, 0);
		}

        if (Gamepad.current != null)
        {
            if (
                Gamepad.current.leftShoulder.wasPressedThisFrame ||
                Gamepad.current.rightShoulder.wasPressedThisFrame ||
                Gamepad.current.leftTrigger.ReadValue() > 0.5f ||
                Gamepad.current.rightTrigger.ReadValue() > 0.5f
            )
            {
                Debug.Log("CONTROLLER SHOOT");
                weaponButtonManager.weapon_button_clicked();
            }
        }
        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is UnityEngine.InputSystem.Controls.ButtonControl button)
                {
                    if (button.wasPressedThisFrame)
                    {
                        Debug.Log(button.name);
                    }
                }
            }
        }
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKey("joystick button " + i))
            {
                weaponButtonManager.weapon_button_clicked();
                break;
            }
        }
        if (
            Input.GetKey("joystick button 6") ||
            Input.GetKey("joystick button 7") ||
            Input.GetKey("joystick button 8") ||
            Input.GetKey("joystick button 9")
        )
        {
            weaponButtonManager.weapon_button_clicked();
        }
	}

	void FixedUpdate()
	{
		// [00:04:41] Calculate the new target position using fixedDeltaTime
		Vector2 targetPosition = (Vector2)player.position + joystickInputs * moveSpeed * Time.fixedDeltaTime;

		// [00:08:21] Clamp the X and Y positions to prevent the player from leaving the screen
		float boundX = Mathf.Clamp(targetPosition.x, -screenBounds.x, screenBounds.x);// Useless!! Just copied from a tutorial, so I decided not to delete and keep it to keep it good - maybe deleting something will lead to an error!
		float boundY = Mathf.Clamp(targetPosition.y, -screenBounds.y, screenBounds.y);// Useless!! Just copied from a tutorial, so I decided not to delete and keep it to keep it good - maybe deleting something will lead to an error!

		// [00:08:54] Apply the clamped target position to the player
		Vector2 finalTargetPosition = new Vector2(boundX, boundY); // Useless!! Just copied from a tutorial, so I decided not to delete and keep it to keep it good - maybe deleting something will lead to an error!
		player.position = targetPosition;
	}
}