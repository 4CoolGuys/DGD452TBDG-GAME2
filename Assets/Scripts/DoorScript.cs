using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Position to move the door when it opens
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0, 2f, 0); // Moves 2 units upwards
    private Vector3 closedPosition;  // The door's initial position
    private bool isOpen = false;     // To check if the door is currently open

    [SerializeField] private float openCloseSpeed = 2f;  // Speed at which the door moves

    private void Start()
    {
        // Store the begining position of the door
        closedPosition = transform.position;
    }

    // This opens the door
    public void OpenDoor()
    {
        if (!isOpen)
        {
            StopAllCoroutines();  // Stop any currently running closing coroutine
            StartCoroutine(MoveDoor(closedPosition + openPositionOffset));  // Start opening the door
            isOpen = true;
            Debug.Log("Door opening...");
        }
    }

    // This closes the door
    public void CloseDoor()
    {
        if (isOpen)
        {
            StopAllCoroutines();  // Stop any currently running opening coroutine
            StartCoroutine(MoveDoor(closedPosition));  // Start closing the door
            isOpen = false;
            Debug.Log("Door closing...");
        }
    }
    
    // Coroutine to move the door smoothly between open and closed positions
    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, openCloseSpeed * Time.deltaTime);
            yield return null;  // Wait for the next frame
        }

        // Snap the position once it's close enough
        transform.position = targetPosition;
    }
}