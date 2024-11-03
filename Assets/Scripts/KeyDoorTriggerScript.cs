using UnityEngine;

public class KeyDoorTriggerScript : MonoBehaviour
{
    private KeyDoorScript doorScript;

    private void Start()
    {
        doorScript = GetComponentInParent<KeyDoorScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerControllerScript player = other.GetComponent<PlayerControllerScript>();
        if (player != null && player.hasKey)
        {
            doorScript.OpenDoor(); // Open the door if the player has the key
        }
    }
}