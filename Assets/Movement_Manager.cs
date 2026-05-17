using UnityEngine;

public class MovementManager : MonoBehaviour
{
    // [00:03:37] References for the variable joystick and player
    public VariableJoystick joystick;
    public Transform player;
    public float targetAngle;
    
    // [00:05:10] Movement speed multiplier
    private float moveSpeed = 6;

    private Vector2 joystickInputs;
    private Vector2 screenBounds;

    void Start()
    {
        // [00:07:30] Calculate the screen bounds based on the main camera to clamp position
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        // [00:03:59] Get directional input from the joystick on Update
        joystickInputs = joystick.Direction;

        // [00:05:53] Handle character rotation based on movement direction
        if (joystickInputs != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(joystickInputs.x, joystickInputs.y) * Mathf.Rad2Deg;
            player.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void FixedUpdate()
    {
        // [00:04:41] Calculate the new target position using fixedDeltaTime
        Vector2 targetPosition = (Vector2)player.position + joystickInputs * moveSpeed * Time.fixedDeltaTime;

        // [00:08:21] Clamp the X and Y positions to prevent the player from leaving the screen
        float boundX = Mathf.Clamp(targetPosition.x, -screenBounds.x, screenBounds.x);
        float boundY = Mathf.Clamp(targetPosition.y, -screenBounds.y, screenBounds.y);

        // [00:08:54] Apply the clamped target position to the player
        Vector2 finalTargetPosition = new Vector2(boundX, boundY);
        player.position = targetPosition;
    }
}