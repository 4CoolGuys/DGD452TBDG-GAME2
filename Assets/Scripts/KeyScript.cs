    using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool isPickedUp = false;
    private Transform player;

    void Update()
    {
        if (isPickedUp && player != null)
        {
            // Follow the player with a slight offset
            Vector3 offsetPosition = player.position + new Vector3(0.5f, 0.5f, 0f);
            transform.position = Vector3.Lerp(transform.position, offsetPosition, Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collides with the key
        PlayerControllerScript playerController = other.GetComponent<PlayerControllerScript>();
        if (playerController != null && !isPickedUp)
        {
            // Attach the key to the player
            isPickedUp = true;
            player = playerController.transform;
            playerController.hasKey = true;

            // Disable the key's collider so it can't be picked up again
            GetComponent<Collider2D>().enabled = false;
        }
    }
}