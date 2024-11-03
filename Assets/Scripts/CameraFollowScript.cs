using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    // Reference to the player's transform.
    public Transform playerTransform;

    // Offset values for adjusting the camera's position.
    public Vector2 offset = new Vector2(2f, 1f); // (x: forward look-ahead, y: vertical offset)
    public float followSpeed = 5f; // Speed at which the camera follows the player

    // Fixed z position of the camera.
    private const float fixedZ = -10f;

    // Reference to the PlayerControllerScript for access to player's direction.
    private PlayerControllerScript playerController;
    private Vector2 lastMoveDirection;

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        // Get the PlayerControllerScript component from the player.
        playerController = playerTransform.GetComponent<PlayerControllerScript>();
        if (playerController == null)
        {
            Debug.LogError("PlayerControllerScript is missing on the player!");
        }
    }

    private void Update()
    {
        if (playerTransform == null || playerController == null) return;

        // Update the direction based on the player's most recent movement.
        lastMoveDirection = playerController.GetLastMoveDirection();

        // Calculate the new camera position with the look-ahead effect.
        Vector3 targetPosition = new Vector3(
            playerTransform.position.x + offset.x * lastMoveDirection.x,
            playerTransform.position.y + offset.y,
            fixedZ // Ensures camera stays at z = -10
        );

        // Smoothly move the camera towards the target position.
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}