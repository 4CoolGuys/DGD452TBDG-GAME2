using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeScript : MonoBehaviour
{
    // When the player collides with this object, the level restarts.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Play defeat sound here

            // Restart the level by reloading the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}