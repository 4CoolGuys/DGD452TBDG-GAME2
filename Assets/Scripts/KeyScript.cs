using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool isPickedUp = false;
    private Transform player;
    public AudioClip pickupSound; // Assign the "Key pickup" sound in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Ensure we have an AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();
    }

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
        // Check if the player collides with the key
        PlayerControllerScript playerController = other.GetComponent<PlayerControllerScript>();
        if (playerController != null && !isPickedUp)
        {
            // Attach the key to the player
            isPickedUp = true;
            player = playerController.transform;
            playerController.hasKey = true;

            // Play pickup sound
            if (audioSource != null && pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            // Disable the key's collider so it can't be picked up again
            GetComponent<Collider2D>().enabled = false;
        }
    }
}