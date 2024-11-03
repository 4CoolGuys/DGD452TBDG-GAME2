using System.Collections;
using UnityEngine;

public class KeyDoorScript : MonoBehaviour
{
    private Collider2D doorCollider;
    private bool isOpen = false;// To check if the door is currently open
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0, 2f, 0); // Moves 2 units upwards
    private Vector3 closedPosition;  // The door's initial position
    [SerializeField] private float openCloseSpeed = 2f;  // Speed at which the door moves

    private void Start()
    {
        doorCollider = GetComponent<Collider2D>();
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerControllerScript player = collision.gameObject.GetComponent<PlayerControllerScript>();
        if (player != null && !player.hasKey)
        {
            // Player collides with the closed door without a key
            Debug.Log("Door is locked. You need a key.");
        }
    }
    
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