using UnityEngine;

public class PushableBoxScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float stopDrag = 10f;    // High drag to stop movement when player is not pushing
    public float moveDrag = 0f;     // Low drag when the player is pushing the box
    public float pushForce = 10f;   // Force applied to the box when pushed by the player

    private bool isBeingPushed = false;

    public bool amBox = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = stopDrag; // Start with high drag to prevent sliding
    }

    void Update()
    {
        // If the box is not being pushed, apply higher drag to stop sliding
        if (!isBeingPushed)
        {
            rb.drag = stopDrag;
        }
        else
        {
            rb.drag = moveDrag;
        }

        // Reset the pushing state
        isBeingPushed = false; 
    }

    // Call this method from the PlayerController when the player is pushing the box
    public void Push(Vector2 direction)
    {
        isBeingPushed = true;
        rb.drag = moveDrag;  // Set drag to low for smoother movement
        rb.AddForce(direction * pushForce);
    }
}