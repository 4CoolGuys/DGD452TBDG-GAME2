using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour
{
    private bool isPlayerNearby = false;  // Check if player is near the door

    // Update is called once per frame
    void Update()
    {
        // Check if the player is near and presses "E" to enter the door
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            LoadNextLevel();
        }
    }

    // Triggered when player enters the door's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if it's the player
        {
            isPlayerNearby = true;
        }
    }

    // Triggered when player exits the door's collider
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    // Loads the next level in the build order
    private void LoadNextLevel()
    {
        // Get the next scene index in the build settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Load the next level if it exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels in the build settings.");
        }
    }
}