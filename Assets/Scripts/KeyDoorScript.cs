using UnityEngine;

public class KeyDoorScript : MonoBehaviour
{
    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the door
        PlayerControllerScript playerController = other.GetComponent<PlayerControllerScript>();
        if (playerController != null)
        {
            if (playerController.hasKey && !isOpen)
            {
                // Open the door
                OpenDoor();
                playerController.hasKey = false; // Player no longer has the key
            }
            else if (!isOpen)
            {
                // Block the player (could add feedback here, like a sound or text popup)
                Debug.Log("The door is locked. You need a key to open it.");
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        Debug.Log("The door opens!");
        
        // Add animations or effects for the door opening here
        // Optionally, disable the collider so the player can pass through
        GetComponent<Collider2D>().enabled = false;
    }
}