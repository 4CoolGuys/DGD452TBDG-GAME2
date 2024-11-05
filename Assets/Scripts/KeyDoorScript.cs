using UnityEngine;

public class KeyDoorScript : MonoBehaviour
{
    private Collider2D doorCollider;
    private bool isOpen = false;
    private bool isMovingUp = false;

    [SerializeField] private float openHeight = 3f; // Height the door will move upwards
    [SerializeField] private float openSpeed = 2f; // Speed of the door opening

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void Update()
    {
        if (isMovingUp)
        {
            // Move the door upwards towards the open position
            transform.position = Vector3.Lerp(transform.position, openPosition, openSpeed * Time.deltaTime);

            // Check if door has reached the open position (or is very close)
            if (Vector3.Distance(transform.position, openPosition) < 0.01f)
            {
                transform.position = openPosition; // Snaps door to open position
                isMovingUp = false; // Stop moving door
            }
        }
    }

    public void OpenDoor(PlayerControllerScript player)
    {
        if (!isOpen)
        {
            isOpen = true;
            isMovingUp = true; // Open door up

            player.hasKey = false; // Player "uses" the key
            DestroyKeyObject(player); // Destroy the key GameObject

            Debug.Log("Door opened and is moving up!");
        }
    }

    private void DestroyKeyObject(PlayerControllerScript player)
    {
        GameObject keyObject = GameObject.FindWithTag("Key");
        if (keyObject != null)
        {
            Destroy(keyObject); // Destroy the key
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerControllerScript player = collision.gameObject.GetComponent<PlayerControllerScript>();
        if (player != null && !player.hasKey)
        {
            Debug.Log("Door is locked. You need a key.");
        }
    }
}
