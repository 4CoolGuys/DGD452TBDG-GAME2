using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Reference to the Door script
    [SerializeField] private DoorScript door;

    private int objectsOnPlate = 0;  // Tracks the number of objects on the plate

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the player or the box
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Box"))
        {
            objectsOnPlate++;
            ActivatePlate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exited is the player or the box
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Box"))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                DeactivatePlate();
            }
        }
    }

    // This method is called when the pressure plate is activated
    private void ActivatePlate()
    {
        if (door != null)
        {
            door.OpenDoor();  // Open the door when the plate is pressed
            Debug.Log("Pressure Plate activated!");
        }
    }

    // This method is called when the pressure plate is deactivated
    private void DeactivatePlate()
    {
        if (door != null)
        {
            door.CloseDoor();  // Close the door when the plate is released
            Debug.Log("Pressure Plate deactivated!");
        }
    }
}