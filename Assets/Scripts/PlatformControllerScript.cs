using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControllerScript : MonoBehaviour
{
    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer;
    private bool isInFirstWorld; // Determines if this platform belongs to the first world or the second
    private bool playerInFirstWorld; // Current world of the player

    // Reference to the player object (set this in the Unity Inspector or find dynamically)
    public GameObject player;

    void Start()
    {
        // Get the platform's collider and SR
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();

        // Check if this platform belongs to the First or Second World based on the tag it has
        isInFirstWorld = gameObject.CompareTag("firstWorld");

        // Get the  world state of the player
        playerInFirstWorld = player.GetComponent<PlayerController>().inFirstWorld;

        UpdatePlatformState();
    }

    void Update()
    {
        // Check current world of the player
        bool currentPlayerWorld = player.GetComponent<PlayerController>().inFirstWorld;

        // If the player's world has changed, update the platform's state
        if (currentPlayerWorld != playerInFirstWorld)
        {
            playerInFirstWorld = currentPlayerWorld;
            UpdatePlatformState();
        }
    }

    void UpdatePlatformState()
    {
        // Check if the player and the platform are in the same world
        if (isInFirstWorld == playerInFirstWorld)
        {
            // Enable platform's collider and reset opacity to full
            platformCollider.enabled = true;
            SetPlatformOpacity(1f); // Full opacity
        }
        else
        {
            // Disable platform's collider and reduce opacity
            platformCollider.enabled = false;
            SetPlatformOpacity(0.5f); // Half opacity
        }
    }

    // Helper method to set the platform's opacity
    void SetPlatformOpacity(float opacity)
    {
        Color color = platformRenderer.color;
        color.a = opacity; // Set the alpha channel to control transparency
        platformRenderer.color = color;
    }
}
